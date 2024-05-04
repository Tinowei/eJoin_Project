window.addEventListener("load", function () {
    restAllPageBtn();
    document.querySelector(".nav-link").classList.add("active")
    document.querySelector(".nav-page").classList.remove("d-none")
    setNavScript();
});

function setNavScript() {
    const nav_links = document.querySelectorAll(".nav-link");
    const nav_pages = document.querySelectorAll(".nav-page");
    nav_links.forEach((item, index) => {
        item.addEventListener("click", (e) => {
            restAllPageBtn();
            e.currentTarget.classList.add("active");
            nav_pages[index].classList.remove("d-none");
        });
    });
}

function restAllPageBtn() {
    document.querySelectorAll(".nav-link").forEach((item) => item.classList.remove("active"));
    document.querySelectorAll(".nav-page").forEach((item) => item.classList.add("d-none"));
}