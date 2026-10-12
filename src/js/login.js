async function login() {
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    try {
        const result = await api("auth/login", "POST", { email, password });

        setToken(result.token);

        const payload = JSON.parse(atob(result.token.split(".")[1]));
        const roleClaim = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        const role = Array.isArray(roleClaim) ? roleClaim[0] : roleClaim;

        localStorage.setItem("role", data.role[0]);


        alert("Inloggad!");

        if (role === "Admin") {
            window.location.href = "admin.html";
        } else {
            window.location.href = "products.html";
        }

    } catch (err) {
        console.error(err);
        alert("Fel vid login");
    }
}
