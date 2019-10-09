var ModalCommons = (function () {

    return {

        code: function (titulo, code, confirmButton = false) {

            var random = Math.random().toString().replace('.', '');
            var texto = '<div id="' + random + '" class="modal fade">                                                                   ' +
                '        <div class="modal-dialog">                                                                                     ' +
                '            <div class="modal-content">                                                                                ' +
                '                <div class="modal-header">                                                                             ' +
                '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>             ' +
                '                    <h4 class="modal-title">' + titulo + '</h4>                                                        ' +
                '                </div>                                                                                                 ' +
                '                <div class="modal-body">                                                                               ' +
                '                    <p>' + code + '</p>                                                                                ' +
                '                </div>                                                                                                 ' +
                '                <div class="modal-footer">                                                                             ' +
                '                    <button type="button" class="btn btn-success btn-confirm">Confirmar</button>                       ' +
                '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>                 ' +
                '                </div>                                                                                                 ' +
                '            </div><!-- /.modal-content -->                                                                             ' +
                '  </div><!-- /.modal-dialog -->                                                                                        ' +
                '</div> <!-- /.modal -->                                                                                                ';

            $('body').append(texto);
            if (confirmButton)
                $('#' + random + ' .btn-confirm').show();
            else
                $('#' + random + ' .btn-confirm').hide();
            
            $('#' + random).modal('show');

            MasksCommons.init();

            $(".modal").on('hidden.bs.modal', function () {
                $(this).remove();
            });

            return random;
        },

        message: function (titulo, texto) {

            var random = Math.random().toString().replace('.', '');
            var texto = '<div id="' + random + '" class="modal fade">                                                       ' +
                '        <div class="modal-dialog">                                                                         ' +
                '            <div class="modal-content">                                                                    ' +
                '                <div class="modal-header">                                                                 ' +
                '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button> ' +
                '                    <h4 class="modal-title">' + titulo + '</h4>                                            ' +
                '                </div>                                                                                     ' +
                '                <div class="modal-body">                                                                   ' +
                '                    <p>' + texto + '</p>                                                                   ' +
                '                </div>                                                                                     ' +
                '                <div class="modal-footer">                                                                 ' +
                '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>     ' +
                '                                                                                                           ' +
                '                </div>                                                                                     ' +
                '            </div><!-- /.modal-content -->                                                                 ' +
                '  </div><!-- /.modal-dialog -->                                                                            ' +
                '</div> <!-- /.modal -->                                                                                    ';

            $('body').append(texto);
            $('#' + random).modal('show');

            $(".modal").on('hidden.bs.modal', function () {
                $(this).remove();
            });

            return random;
        },

        close: function (modalId) {
            $("#" + modalId).modal("hide");
        }
    }

})(jQuery);

