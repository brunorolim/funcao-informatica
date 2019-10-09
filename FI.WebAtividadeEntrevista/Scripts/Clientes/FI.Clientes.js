
var Clientes = (function () {

    return {

        init: function () {

            MasksCommons.init();

            $('#formCadastro #btnBeneficiarios').click(function () {
                Beneficiarios.modalBeneficiarios();
            });

            $('#formCadastro').submit(function (e) {
                e.preventDefault();
                $.ajax({
                    url: urlPost,
                    method: "POST",
                    data: {
                        "Nome": $(this).find("#Nome").val(),
                        "CEP": $(this).find("#CEP").val(),
                        "Email": $(this).find("#Email").val(),
                        "Sobrenome": $(this).find("#Sobrenome").val(),
                        "CPF": $(this).find("#CPF").val(),
                        "Nacionalidade": $(this).find("#Nacionalidade").val(),
                        "Estado": $(this).find("#Estado").val(),
                        "Cidade": $(this).find("#Cidade").val(),
                        "Logradouro": $(this).find("#Logradouro").val(),
                        "Telefone": $(this).find("#Telefone").val(),
                        "Beneficiarios": Beneficiarios.data()
                    },
                    error:
                        function (r) {
                            if (r.status == 400)
                                ModalCommons.message("Ocorreu um erro", r.responseJSON);
                            else if (r.status == 500)
                                ModalCommons.message("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                        },
                    success:
                        function (r) {
                            ModalCommons.message("Sucesso!", r);
                            $("#formCadastro")[0].reset();
                            Beneficiarios.init();
                        }
                });
            })

        }
        
    }

}) (jQuery);

$(function () {
    Clientes.init();
});