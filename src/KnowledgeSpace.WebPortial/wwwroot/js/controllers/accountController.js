var accountController = function () {

    this.initialize = function () {

        registerEvents();

    };

    function registerEvents() {
        $('#btn_add_attachment').on('click', function () {
            $('#attachment_items').prepend('<p><input type="file" name="attachments" /></p>');
            return false;
        });
        $('body').on('click', '#img-captcha', function (e) {
            resetCaptchaImage('img-captcha');
        });
    }
};