async function loadMyOrders() {
    const orders = await api("orders/mine", "GET");
    console.log(orders);

    const list = document.getElementById("ordersList");
    list.innerHTML = "";

    orders.forEach(o => {
        const productsHtml = o.products
            .map(p => `
                <li>
                    <img src="${p.imageUrl}" style="max-width:100px;">
                    ${p.name} - ${p.price} kr
                </li>
            `)
            .join("");

        list.innerHTML += `
            <div class="card">
                <h3>Order #${o.id}</h3>
                <p><strong>Totalpris:</strong> ${o.totalPrice} kr</p>
                <p><strong>Datum:</strong> ${o.createdAt}</p>
                <p><strong>Produkter:</strong></p>
                <ul>${productsHtml}</ul>
            </div>
        `;
    });
}

window.onload = loadMyOrders;
