var MasksCommons = (function () {

    return {

        init: function () {

            var foneMaskBehavior = function (val) {
                return val.replace(/\D/g, '').length <= 10 ? '(00) 0000-00009' : '(00) 00000-0000';
            },
            foneOptions = {
                onKeyPress: function (val, e, field, options) {
                    field.mask(foneMaskBehavior.apply({}, arguments), options);
                }
            };

            $('.foneMask').mask(foneMaskBehavior, foneOptions);

            $('.cpfMask').mask('000.000.000-00');
            $('.cepMask').mask('00000-000');
        }
    }

})(jQuery);
