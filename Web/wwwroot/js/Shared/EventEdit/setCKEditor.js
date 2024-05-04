window.addEventListener("load", function () {
    setCKEditor();
})

function setCKEditor() {
    ClassicEditor.create(document.querySelector("#editor"))
        .then((editor) => {
            console.log(editor);
        })
        .catch((error) => {
            console.error(error);
        });
}