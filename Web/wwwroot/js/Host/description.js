let save_btn = document.querySelector(".save-btn");

save_btn.addEventListener("click", () => {
    window.removeEventListener("beforeunload", beforeUnloadHandler) //�H���W��k����h�����ƥ�
    window.location.assign("TicketSetting");
});

window.addEventListener('beforeunload', beforeUnloadHandler); //���W�ƥ�

function beforeUnloadHandler(event) { //�w�q�ƥ󤺮e�A�Y�O�ΰΦW�禡�L�k������ť
    event.preventDefault();
    event.returnValue = "�O�_�T�w�n���}����?";
}

