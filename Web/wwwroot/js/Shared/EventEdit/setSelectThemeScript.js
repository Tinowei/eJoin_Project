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
                //���o��U�D�Ddiv�U������input ThemeId
                var selectThemeId = item.querySelector("input[name='ThemeId']").value;
                //�s��ܦh���D�D�����G
                var ids = $("#ThemeIdList").val();
                //��ثe��ܪ��D�D������ŭ�
                if (ids != '') {
                    //�s�i�Ӫ��Ĥ@�ӥD�D
                    var attachThemeArray = ids.split(',');
                    //���s�b�~�s�W
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
