let save_btn = document.querySelector(".save-btn");

save_btn.addEventListener("click", () => {
    window.removeEventListener("beforeunload", beforeUnloadHandler) //以正規方法跳轉則移除事件
    window.location.assign("TicketSetting");
});

window.addEventListener('beforeunload', beforeUnloadHandler); //掛上事件

function beforeUnloadHandler(event) { //定義事件內容，若是用匿名函式無法取消監聽
    event.preventDefault();
    event.returnValue = "是否確定要離開網站?";
}

