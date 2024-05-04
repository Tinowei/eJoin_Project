let agree_checkbox = document.querySelector("#agree");
let save_btn = document.querySelector(".save-btn");

function beforeUnloadHandler (event) { //�w�q�ƥ󤺮e�A�Y�O�ΰΦW�禡�L�k������ť
    event.preventDefault();
    event.returnValue = "�O�_�T�w�n���}����?";
}

window.addEventListener('beforeunload', beforeUnloadHandler); //���W�ƥ�

agree_checkbox.addEventListener("change", () => {
    if (agree_checkbox.checked) {
        save_btn.classList.remove("off");
    } else {
        save_btn.classList.add("off");
    }
});

save_btn.addEventListener("click", () => {
    window.removeEventListener("beforeunload", beforeUnloadHandler) //�H���W��k����h�����ƥ�
    window.location.assign("EditEvent");
});
