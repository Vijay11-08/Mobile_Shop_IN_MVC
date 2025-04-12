document.getElementById("payButton").addEventListener("click", async function () {
    let response = await fetch("/Payment/CreateOrder", { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify({ amount: 500 }) });
    let data = await response.json();

    let options = {
        key: "rzp_test_x8tV5oSUixLmbV",
        amount: 50000,
        currency: "INR",
        name: "Mobile Shop",
        order_id: data.orderId,
        handler: function (response) {
            alert("Payment Successful! Payment ID: " + response.razorpay_payment_id);
        }
    };

    let rzp = new Razorpay(options);
    rzp.open();
});