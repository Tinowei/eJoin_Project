window.addEventListener("load", function () {
    setSelectThemeScript();
});

function setSelectThemeScript() {
    const theme_cards = document.querySelectorAll(".theme-card");
    const save_btn = document.querySelector(".select-theme-content .save-btn");
    let selectedCount = 0;
    theme_cards.forEach((item) => {
        item.addEventListener("click", (e) => {
            if (e.currentTarget.classList.contains("selected")) {
                e.currentTarget.classList.remove("selected");
                selectedCount--;
            } else if (!e.currentTarget.classList.contains("selected") && selectedCount < 2) {
                e.currentTarget.classList.add("selected");
                selectedCount++;
                //取得當下主題div下的隱藏input ThemeId
                var selectThemeId = item.querySelector("input[name='ThemeId']").value;
                //存選擇多筆主題的結果
                var ids = $("#ThemeIdList").val();
                //當目前選擇的主題不等於空值
                if (ids != '') {
                    //存進來的第一個主題
                    var attachThemeArray = ids.split(',');
                    //不存在才新增
                    if (!attachThemeArray.includes(selectThemeId)) {

                        attachThemeArray.push(selectThemeId);
                        $("#ThemeIdList").val(attachThemeArray.join(','));
                    }
                } else {
                    $("#ThemeIdList").val(selectThemeId);
                }

                save_btn.classList.remove("off");
            }
            if (selectedCount == 0) save_btn.classList.add("off");
               
            console.log("selectedCount=" + selectedCount);
        });
    });
}
