let agreeCheckbox = document.getElementById("agree-btn");
let nextBtn = document.querySelector(".next-btn");

//預存參加人資訊的字串
let participantInfo = "";

// document.getElementById("agree-btn").addEventListener("click", function () {
//
//     if (agreeCheckbox.checked) {
//         // nextBtn.removeAttribute("disabled");
//     } else {
//         // showReadAndAgreeAlert();
//         // nextBtn.setAttribute("disabled", "disabled");
//     }
// });

function showReadAndAgreeAlert(){
    Swal.fire({
        icon: 'warning',
        text: '請先閱讀並同意eJoin條款',
        showConfirmButton: true,
        confirmButtonText:'確認',
        heightAuto: false,
        customClass: {
            title: 'S2_title_class',
            content: 'S2_content_class',
        }
    })
}




const form = document.getElementById("fillForm");
//form.addEventListener("submit",Check);
// document.querySelector('.next-btn').addEventListener('click',Check);

function isEmail(email) {
    let regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email)) {
        return false;
    } else {
        return true;
    }
}

function isPhone(phone) {
    let regex = /^09[0-9]{8}$/;
    if (!regex.test(phone)) {
        return false;
    } else {
        return true;
    }
}

function StoreEventSource() {
    let stringSource = ""; // 初始化一個空字串來累加資訊來源
    const sources = document.querySelectorAll('.source input[type="checkbox"]');
    for (let s of sources) {
        const opotionalLabel = s.nextElementSibling; // 使用 nextElementSibling 來獲取對應的 label
        if (s.checked) {
            stringSource += (stringSource ? "," : "") + opotionalLabel.getAttribute("data-number"); // 如果 stringSource 不是空的，則在前面加上逗號和空格
        }
    }
    return stringSource;
}
//表單驗證
function Check(e) {
    e.preventDefault();
    const fields = document.querySelectorAll(".field");
    const values = {};
    let hasError = false;
    for (f of fields) {
        const isText = f.classList.contains("txt");
        const isCheckbox = f.classList.contains("checkbox");
        const isRequired = f.classList.contains("required");
        const remind = f.querySelector(".remind");
        const remind2 = f.querySelector(".remind2");

        if (isText) {
            const input = f.querySelector('input[type="text"]');
            const inputValue = input.value;
            if (isRequired) {
                const name = f.querySelector("input").getAttribute("name");
                //檢查phone欄位
                if (name === "ParticipantPhone") {
                    if (isPhone(inputValue)) {
                        remind.classList.remove("show");
                        remind2.classList.remove("show");
                        values[input.name] = inputValue;
                    } else if (inputValue) {
                        remind.classList.remove("show");
                        remind2.classList.add("show");
                        hasError = true;
                    } else if (!inputValue) {
                        remind.classList.add("show");
                        remind2.classList.remove("show");
                        hasError = true;
                    }
                }
                //email 欄位確認
                else if (name === "ParticipantEmail") {
                    if (isEmail(inputValue)) {
                        remind.classList.remove("show");
                        remind2.classList.remove("show");
                        values[input.name] = inputValue;
                    } else if (inputValue) {
                        remind.classList.remove("show");
                        remind2.classList.add("show");
                        hasError = true;
                    } else if (!inputValue) {
                        remind.classList.add("show");
                        remind2.classList.remove("show");
                        hasError = true;
                    }
                }
                // 其他必填欄位有填寫時
                else if (inputValue) {
                    remind.classList.remove("show");
                    values[input.name] = inputValue;
                }
                //必填欄位沒填寫時的處理
                else if (!inputValue) {
                    remind.classList.add("show");
                    hasError = true;
                }
                //不是必填欄位時，若有輸入資料，就直接儲存
                else if (inputValue) {
                    values[input.name] = inputValue;
                }
            }
        }
        if (isCheckbox) {
            const types = f.querySelector('input[type="checkbox"]');
            let checkType;
            //若有勾選，則存取勾選項目的 value
            if (type.checked) {
                checkType = type;
                // remind.classList.remove("show");
                // values[type.name] = checkType.value;
            }
            //若無勾選擇出現提醒
            if (!checkType) {
                remind.classList.add("show");
                hasError = true;
            }
        }
    }
    if (!hasError) {
        // values.source = StoreEventSource();
        // values.push({ value: StoreEventSource() });
        values.Sources = StoreEventSource();
        participantInfo = JSON.stringify(values);
        console.log(participantInfo);
        // alert(JSON.stringify(values));
        return true;
    }
    return false;
}

//帶著eventId進入Payment頁面
document.querySelector(".next-btn").onclick = function (e) {
    if (Check(e)) {
        ChangeLearnedFromValue();
        if(!agreeCheckbox.checked){
            showReadAndAgreeAlert();
        }else{
            document.querySelector("#fillForm").submit();
        }
    }
};

//重新報名跳轉至首頁
document.querySelectorAll(".restart-btn").forEach((btn) => {
    btn.addEventListener("click", () => {
        window.location.assign(`/Home/Event/${eventId}`);
    });
});

//點擊第一步step回上一頁

document.querySelector(".step-container:first-child .step").onclick = function () {
    window.location.assign(`/Register/Index?EventId=${eventId}`);
};

// By Adam

// 來源的Checkbox的NodeList
const source_inputs = document.querySelectorAll(".source input");

window.addEventListener("load", function () {
    OnCheckSourceCheckbox();
});

function ChangeLearnedFromValue() {
    const learnedFrom_input = document.querySelector("#learnedFrom-input");
    let checkedArray = [];
    source_inputs.forEach((node) => {
        if (node.checked == true) checkedArray.push(node.value);
    });
    learnedFrom_input.value = JSON.stringify(checkedArray);
}

// 把Source的紀錄顯示在Checkbox上
function OnCheckSourceCheckbox() {
    source_inputs.forEach((node) => {
        if (learnFrom.includes(node.value)) node.checked = true;
    });
}
