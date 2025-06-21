const apiBase = "https://localhost:7251/api/Auth"; // Укажи свой адрес API

document.getElementById("registerForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const email = document.getElementById("registerEmail").value;
    const password = document.getElementById("registerPassword").value;

    const response = await fetch(`${apiBase}/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password })
    });

    const data = await response.json();

    if (response.ok) {
        alert("Регистрация успешна!");
    } else {
        alert("Ошибка регистрации: " + JSON.stringify(data));
    }
});

document.getElementById("loginForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const email = document.getElementById("emailaddress").value;
    const password = document.getElementById("password").value;

    try {
        const response = await fetch(`${apiBase}/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password })
        });

        const data = await response.json();

        if (response.ok) {
            localStorage.setItem("token", data.token);
            alert("Вход успешен!");
            // window.location.href = "/dashboard.html"; // можно сделать редирект
        } else {
            alert("Ошибка входа: " + (data?.message || JSON.stringify(data)));
        }
    } catch (err) {
        alert("Ошибка соединения с сервером");
        console.error(err);
    }
});

document.getElementById("authorRegisterForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const username = document.getElementById("authorUsername").value;
    const email = document.getElementById("authorEmail").value;
    const password = document.getElementById("authorPassword").value;
    const portfolioUrl = document.getElementById("portfolioUrl").value;

    const response = await fetch(`${apiBase}/register-author`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, email, password, portfolioUrl })
    });

    const data = await response.json();

    if (response.ok) {
        alert("Автор зарегистрирован!");
    } else {
        alert("Ошибка регистрации автора: " + JSON.stringify(data));
    }
});
