async function loadProducts() {
    try {
        const search = document.getElementById("search").value || "";

        const products = await api("products", "GET");

        const list = document.getElementById("productList");
        list.innerHTML = "";

        products
            .filter(p => p.name.toLowerCase().includes(search.toLowerCase()))
            .forEach(p => {
                list.innerHTML += `
                    <div class="card">
                        <img src="${p.imageUrl}" style="max-width:150px;">
                        <h3>${p.name}</h3>
                        <p>${p.price} kr</p>
                        <button onclick="createOrder(${p.id})">Köp</button>
                    </div>
                `;
            });

    } catch (err) {
        console.error(err);
        alert("Kunde inte hämta produkter");
    }
}

async function createOrder(productId) {
    const token = localStorage.getItem("jwt");

    if (!token) {
        alert("Du måste vara inloggad för att lägga en order.");
        window.location.href = "login.html";
        return;
    }

    try {
        await api("orders", "POST", { productIds: [productId] });
        alert("Order skapad!");
    } catch (err) {
        console.error(err);
        alert("Order kunde inte skapas");
    }
}

window.onload = loadProducts;
