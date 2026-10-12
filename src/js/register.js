async function register() {
    const email = document.getElementById("regEmail").value;
    const password = document.getElementById("regPassword").value;

    try {
        await api("auth/register", "POST", {
            email,
            password
        });

        alert("Konto skapat!");
        window.location.href = "login.html";

    } catch (err) {
        console.error(err);
        alert("Fel vid registrering");
    }
}
