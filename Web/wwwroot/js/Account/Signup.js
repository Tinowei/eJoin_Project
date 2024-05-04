function myFormCheck() {
    // 獲取表單中的輸入值
    const userEmail = document.getElementById('useremail').value.trim();
    const username = document.getElementById('username').value.trim();
    const phoneNumber = document.getElementById('phonenumber').value.trim();
    const password = document.getElementById('password').value.trim();

    // 電子郵件的正則表達式
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    // 手機號碼的正則表達式
    const phoneNumberRegex = /^09\d{8}$|^09\d{2}-\d{3}-\d{3}$|^09\d{2}-\d{6}$/;
    // 密碼的正則表達式
    const passwordRegex = /^[a-zA-Z0-9]+$/;

    // 檢查電子郵件是否有效
    if (!emailRegex.test(userEmail)) {
        Swal.fire({
            icon: 'warning',
            title: '【格式提示】',
            text: '「註冊信箱」的格式不正確。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
        document.getElementById('useremail').classList.add('red-border');
        return;
    }

    // 檢查姓名是否有效
    if (username === '') {
        Swal.fire({
            icon: 'warning',
            title: '【必填項目提示】',
            text: '請輸入「姓名」。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
        document.getElementById('username').classList.add('red-border');
        return;
    }

    // 檢查手機號碼是否有效
    if (!phoneNumberRegex.test(phoneNumber)) {
        Swal.fire({
            icon: 'warning',
            title: '【格式提示】',
            text: '「手機」的格式不正確。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
        document.getElementById('phonenumber').classList.add('red-border');
        return;
    }

    // 檢查密碼是否有效
    if (password === '' || password.length < 7) {
        Swal.fire({
            icon: 'warning',
            title: '【長度提示】',
            text: '「密碼」的長度，不能少於7個字元。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
        document.getElementById('password').classList.add('red-border');
        return;
    }

    // 如果所有檢查都通過，則提交表單
    document.querySelector('form').submit();
}

document.querySelector('#submit-btn').addEventListener('click', function (event) {
    event.preventDefault();
    myFormCheck();
})

window.addEventListener('load', function () {
    if (return_status == 1) {
        Swal.fire({
            icon: 'warning',
            title: '重覆註冊',
            text: '此信箱已經註冊過，請直接登入!',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
    }
    else if (return_status == -1) {
        Swal.fire({
            icon: 'warning',
            title: '註冊失敗',
            text: '註冊時發生錯誤，請聯絡工作人員',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
    }
})


document.querySelector("#verificationSuccess").style.display = "none";
document.querySelector("#showVerification").style.display = "block";

function disableInputs() {
    document.getElementById("username").disabled = true;
    document.getElementById("phonenumber").disabled = true;
    document.getElementById("password").disabled = true;
}

// 啟用輸入框
function enableInputs() {
    document.getElementById("username").disabled = false;
    document.getElementById("phonenumber").disabled = false;
    document.getElementById("password").disabled = false;
}

// 當頁面載入時，禁用輸入框
disableInputs();

let verifyNumber;
function GetVerifyNumber() {
    // 定義所有可能的字符
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    let result = '';

    // 生成六位數的隨機文字
    for (let i = 0; i < 6; i++) {
        // 生成一個隨機索引
        const randomIndex = Math.floor(Math.random() * characters.length);
        // 從字符串中選取一個字符
        const randomCharacter = characters[randomIndex];
        // 將選取的字符添加到結果字符串中
        result += randomCharacter;
    }

    return result;
}

document.querySelector("#sendEmail").addEventListener("click", function () {
    this.disabled = true;
    this.innerText = "60秒後可重新發送";
    let ele = this;
    let countdown = 60;
    let countdownInterval = setInterval(function () {
        countdown--;
        ele.innerText = `${countdown}秒後可重新發送`;

        // 當倒數結束時，清除計時器並啟用按鈕
        if (countdown <= 0) {
            clearInterval(countdownInterval);
            ele.disabled = false;
            ele.innerText = '重新發信';
        }
    }, 1000);
    //產生6位數驗證碼
    verifyNumber = GetVerifyNumber();

    //打api傳送email & 驗證碼
    CallSendVerifyEmailApi();
})


document.querySelector("#verifyBtn").addEventListener("click", function () {
    //按下這顆驗證按鍵以後會觸發的事:
    //核對輸入框中的數字與當初產生的驗證碼是否相符合

    let verificationCodeInput = document.getElementById("verificationCodeInput").value;

    // 檢查輸入的驗證碼是否與預期的驗證碼相符
    if (verificationCodeInput === verifyNumber) {
        // 驗證成功，顯示驗證成功訊息並隱藏驗證輸入區塊
        document.querySelector("#verificationSuccess").style.display = "block";
        document.getElementById("verificationSuccess").textContent = "驗證成功！";
        document.querySelector("#showVerification").style.display = "none";

        enableInputs();

    } else {
        document.getElementById("verificationSuccess").style.display = "block";
        document.getElementById("verificationSuccess").textContent = "驗證失敗，請重新輸入";
        disableInputs();

    }
})

const inputEmail = document.querySelector("#useremail");

async function CallSendVerifyEmailApi() {
    let requestObject = {
        "recipientAddress": inputEmail.value,
        "VerifyNumber": verifyNumber
    };
    console.log(requestObject);
    let requestBoby = JSON.stringify(requestObject);
    try {
        let options = {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: requestBoby,
        };
        let response = await fetch("/api/Signup", options);
        let data = await response.json();

    } catch (err) { }
}

