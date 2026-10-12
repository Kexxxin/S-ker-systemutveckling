function setToken(t) {
    localStorage.setItem("jwt", t);
}

function getToken() {
    return localStorage.getItem("jwt");
}

const API_URL = "http://localhost:5250/api/";

async function api(path, method = "GET", body = null) {
    const headers = {
        "Content-Type": "application/json"
    };

    const token = getToken();
    if (token) {
        headers["Authorization"] = "Bearer " + token;
    }

    const res = await fetch(API_URL + path, {
        method,
        headers,
        body: body ? JSON.stringify(body) : null
    });

    if (!res.ok) {
        const text = await res.text();
        throw new Error(text || ("API error: " + res.status));
    }

    try {
        return await res.json();
    } catch {
        return null;
    }
}

function logout() {
    localStorage.removeItem("jwt");
    localStorage.removeItem("role");
    alert("Du är utloggad");
    window.location.href = "login.html";
}
