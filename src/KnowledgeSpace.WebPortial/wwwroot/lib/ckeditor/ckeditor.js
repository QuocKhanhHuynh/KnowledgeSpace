
ClassicEditor.create(document.querySelector('#txt_workaround'), {}).then(editor => {
    window.editor = editor;
}).catch(err => {
    console.error(err.stack)
});
