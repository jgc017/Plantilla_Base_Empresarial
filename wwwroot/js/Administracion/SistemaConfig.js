// Obtiene el token antifalsificacion generado en VwSistemaConfig.cshtml.
const defaultsSistemaConfig = {
    logoUrl: "/img/IMAGENIA.png",
    faviconUrl: "/favicon.ico",
    loginBackgroundUrl: "/img/auth-background.svg"
};

function getCsrfToken() {
    return document.getElementById("csrfToken")?.value || "";
}

// Wrapper de fetch usado por el flujo de configuracion visual.
function secureFetch(url, options = {}) {
    const method = (options.method || "GET").toUpperCase();
    const headers = new Headers(options.headers || {});

    if (method !== "GET" && method !== "HEAD" && method !== "OPTIONS") {
        headers.set("X-CSRF-TOKEN", getCsrfToken());
    }

    return fetch(url, {
        ...options,
        headers,
        credentials: "same-origin"
    });
}

// Normaliza respuestas del API y maneja autenticacion/autorizacion.
async function parseJsonResponse(response) {
    if (response.status === 401) {
        window.location.href = "/Account/Login";
        return null;
    }

    if (response.status === 403) {
        mostrarAlerta("advertencia", "Acceso denegado", "No tienes permisos para realizar esta accion.");
        return null;
    }

    const data = await response.json().catch(() => null);
    if (!response.ok && !data) {
        mostrarAlerta("error", "Error", "No se pudo procesar la solicitud.");
        return null;
    }

    return data;
}

document.addEventListener("DOMContentLoaded", () => {
    F_GetSistemaVisualConfig();

    document.getElementById("BtnGuardarSistemaConfig")?.addEventListener("click", P_UdpSistemaVisualConfig);
    document.getElementById("BtnRestaurarSistemaConfig")?.addEventListener("click", () => pintarFormulario(defaultsSistemaConfig));
    document.getElementById("btnSubirLogoSistema")?.addEventListener("click", () => P_UploadImagenSistema("logo", "fileLogoSistema", "txtLogoSistema", "previewLogoSistema"));
    document.getElementById("btnSubirFaviconSistema")?.addEventListener("click", () => P_UploadImagenSistema("favicon", "fileFaviconSistema", "txtFaviconSistema", "previewFaviconSistema"));
    document.getElementById("btnSubirFondoLoginSistema")?.addEventListener("click", () => P_UploadImagenSistema("loginbackground", "fileFondoLoginSistema", "txtFondoLoginSistema", "previewFondoLoginSistema"));

    document.getElementById("txtLogoSistema")?.addEventListener("input", () => mostrarPreviewImagen("txtLogoSistema", "previewLogoSistema"));
    document.getElementById("txtFaviconSistema")?.addEventListener("input", () => mostrarPreviewImagen("txtFaviconSistema", "previewFaviconSistema"));
    document.getElementById("txtFondoLoginSistema")?.addEventListener("input", () => mostrarPreviewImagen("txtFondoLoginSistema", "previewFondoLoginSistema"));
});

// F_GetSistemaVisualConfig: consulta y pinta la configuracion guardada.
function F_GetSistemaVisualConfig() {
    secureFetch("/api/SistemaConfigApi/F_GetSistemaVisualConfig")
        .then(parseJsonResponse)
        .then(data => {
            if (!data || !data.ok) return;
            pintarFormulario(data.data);
        })
        .catch(() => mostrarAlerta("advertencia", "Error inesperado", "No se pudo cargar la configuracion visual."));
}

// P_UdpSistemaVisualConfig: guarda las rutas actuales de logo, favicon y fondo.
function P_UdpSistemaVisualConfig() {
    mostrarConfirmacion(
        "Guardar configuracion visual?",
        "Los cambios se veran al recargar las paginas del sistema.",
        (confirmado) => {
            if (!confirmado) return;

            subirImagenesPendientes()
                .then(imagenesListas => {
                    if (!imagenesListas) return null;

                    const payload = obtenerPayloadSistemaConfig();
                    if (!payload) return null;

                    return secureFetch("/api/SistemaConfigApi/P_UdpSistemaVisualConfig", {
                        method: "PUT",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify(payload)
                    });
                })
                .then(response => response ? parseJsonResponse(response) : null)
                .then(data => {
                    if (!data) return;

                    if (data.ok) {
                        mostrarAlerta("exito", "Actualizado", data.mensaje);
                        pintarFormulario(data.data);
                        actualizarLogoLoader(data.data.logoUrl);
                    } else {
                        mostrarAlerta("error", "Error", data.mensaje);
                    }
                })
                .catch(() => mostrarAlerta("advertencia", "Error inesperado", "No se pudo guardar la configuracion."));
        }
    );
}

// P_UploadImagenSistema: sube la imagen seleccionada y asigna la ruta devuelta al input correspondiente.
function P_UploadImagenSistema(tipoImagen, inputArchivoId, inputRutaId, previewId) {
    const archivo = document.getElementById(inputArchivoId)?.files?.[0];
    if (!archivo) {
        mostrarAlerta("advertencia", "Imagen requerida", "Selecciona una imagen antes de subirla.");
        return;
    }

    subirImagenSiPendiente(tipoImagen, inputArchivoId, inputRutaId, previewId, true);
}

// subirImagenesPendientes: antes de guardar, carga cualquier archivo seleccionado en los tres campos.
function subirImagenesPendientes() {
    return subirImagenSiPendiente("logo", "fileLogoSistema", "txtLogoSistema", "previewLogoSistema")
        .then(ok => ok ? subirImagenSiPendiente("favicon", "fileFaviconSistema", "txtFaviconSistema", "previewFaviconSistema") : false)
        .then(ok => ok ? subirImagenSiPendiente("loginbackground", "fileFondoLoginSistema", "txtFondoLoginSistema", "previewFondoLoginSistema") : false);
}

function subirImagenSiPendiente(tipoImagen, inputArchivoId, inputRutaId, previewId, mostrarExito = false) {
    const inputArchivo = document.getElementById(inputArchivoId);
    const archivo = inputArchivo?.files?.[0];

    if (!archivo) {
        return Promise.resolve(true);
    }

    const formData = new FormData();
    formData.append("imagen", archivo);
    formData.append("tipoImagen", tipoImagen);

    return secureFetch("/api/SistemaConfigApi/P_UploadImagenSistema", {
        method: "POST",
        body: formData
    })
        .then(parseJsonResponse)
        .then(data => {
            if (!data) return;

            if (data.ok) {
                document.getElementById(inputRutaId).value = data.data;
                mostrarPreviewImagen(inputRutaId, previewId);
                inputArchivo.value = "";
                if (mostrarExito) {
                    mostrarAlerta("exito", "Imagen cargada", data.mensaje);
                }
                return true;
            } else {
                mostrarAlerta("error", "No fue posible subir", data.mensaje);
                return false;
            }
        })
        .catch(() => {
            mostrarAlerta("advertencia", "Error inesperado", "No se pudo subir la imagen.");
            return false;
        });
}

function obtenerPayloadSistemaConfig() {
    const logoUrl = valorCampo("txtLogoSistema");
    const faviconUrl = valorCampo("txtFaviconSistema");
    const loginBackgroundUrl = valorCampo("txtFondoLoginSistema");

    if (!logoUrl) { mostrarError("txtLogoSistema", "El logo es obligatorio"); return null; }
    if (!faviconUrl) { mostrarError("txtFaviconSistema", "El favicon es obligatorio"); return null; }
    if (!loginBackgroundUrl) { mostrarError("txtFondoLoginSistema", "El fondo del login es obligatorio"); return null; }

    return { logoUrl, faviconUrl, loginBackgroundUrl };
}

function pintarFormulario(config) {
    document.getElementById("txtLogoSistema").value = config.logoUrl || defaultsSistemaConfig.logoUrl;
    document.getElementById("txtFaviconSistema").value = config.faviconUrl || defaultsSistemaConfig.faviconUrl;
    document.getElementById("txtFondoLoginSistema").value = config.loginBackgroundUrl || defaultsSistemaConfig.loginBackgroundUrl;

    mostrarPreviewImagen("txtLogoSistema", "previewLogoSistema");
    mostrarPreviewImagen("txtFaviconSistema", "previewFaviconSistema");
    mostrarPreviewImagen("txtFondoLoginSistema", "previewFondoLoginSistema");
}

function valorCampo(id) {
    return document.getElementById(id).value.trim();
}

function mostrarPreviewImagen(inputRutaId, previewId) {
    const ruta = valorCampo(inputRutaId);
    const preview = document.getElementById(previewId);

    if (!ruta) {
        preview.removeAttribute("src");
        preview.classList.add("d-none");
        return;
    }

    preview.src = ruta;
    preview.classList.remove("d-none");
}

function actualizarLogoLoader(logoUrl) {
    const meta = document.querySelector("meta[name='app-logo']");
    if (meta) meta.setAttribute("content", logoUrl);

    const loaderLogo = document.querySelector(".global-loading-image");
    if (loaderLogo) loaderLogo.src = logoUrl;
}
