const userRole = localStorage.getItem("role");

if (userRole !== "Admin") {
    alert("Du har inte behörighet att se denna sida.");
    window.location.href = "products.html";
    throw new Error("Not authorized");
}

async function loadAdminProducts() {
    try {
        const products = await api("products", "GET");

        const list = document.getElementById("adminProductList");
        list.innerHTML = "";

        products.forEach(p => {
            list.innerHTML += `
                <div class="card">
                    <h3>${p.name}</h3>
                    <p>${p.price} kr</p>
                    <img src="${p.imageUrl}" style="max-width:150px;">
                    
                    <h4>Uppdatera</h4>
                    <input id="name-${p.id}" value="${p.name}">
                    <input id="price-${p.id}" type="number" value="${p.price}">
                    <input id="image-${p.id}" value="${p.imageUrl}">
                    <button onclick="updateProduct(${p.id})">Spara</button>

                    <h4>Ta bort</h4>
                    <button onclick="deleteProduct(${p.id})" style="background:red;color:white;">
                        Ta bort produkt
                    </button>
                </div>
            `;
        });

    } catch (err) {
        console.error(err);
        alert("Kunde inte hämta produkter");
    }
}

async function createProduct() {
    const name = document.getElementById("newName").value;
    const price = Number(document.getElementById("newPrice").value);
    const imageUrl = document.getElementById("newImageUrl").value;

    try {
        await api("products", "POST", { name, price, imageUrl });
        alert("Produkt skapad!");
        loadAdminProducts();
    } catch (err) {
        console.error(err);
        alert("Kunde inte skapa produkt");
    }
}

async function updateProduct(id) {
    const name = document.getElementById(`name-${id}`).value;
    const price = Number(document.getElementById(`price-${id}`).value);
    const imageUrl = document.getElementById(`image-${id}`).value;

    try {
        await api(`products/${id}`, "PUT", { name, price, imageUrl });
        alert("Produkt uppdaterad!");
        loadAdminProducts();
    } catch (err) {
        console.error(err);
        alert("Kunde inte uppdatera produkt");
    }
}

async function deleteProduct(id) {
    try {
        await api(`products/${id}`, "DELETE");
        alert("Produkt borttagen!");
        loadAdminProducts();
    } catch (err) {
        console.error(err);
        alert("Kunde inte ta bort produkt");
    }
}

window.onload = loadAdminProducts;
