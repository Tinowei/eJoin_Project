let agree_checkbox = document.querySelector("#agree");
let save_btn = document.querySelector(".save-btn");

function beforeUnloadHandler (event) { //定義事件內容，若是用匿名函式無法取消監聽
    event.preventDefault();
    event.returnValue = "是否確定要離開網站?";
}

window.addEventListener('beforeunload', beforeUnloadHandler); //掛上事件

agree_checkbox.addEventListener("change", () => {
    if (agree_checkbox.checked) {
        save_btn.classList.remove("off");
    } else {
        save_btn.classList.add("off");
    }
});

save_btn.addEventListener("click", () => {
    window.removeEventListener("beforeunload", beforeUnloadHandler) //以正規方法跳轉則移除事件
    window.location.assign("EditEvent");
});
