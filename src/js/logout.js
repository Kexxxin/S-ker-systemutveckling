function logout() {
    localStorage.removeItem("jwt");
    localStorage.removeItem("role");
    alert("Du är nu utloggad!");
    window.location.replace("login.html");
}
