
let Beneficiarios = (function () {

    let idModalData = null;
    let beneficiarioIndex = null;
    let beneficiarios = [];

    return {

        init: function () {
            beneficiarios = [];
        },

        data: function () {
            return beneficiarios;
        },

        adicionar: function (id, cpf, nome) {

            let indexAlterar = null;
            let cpfExistente = false;
            $.each(beneficiarios, function (index, value) {
                if (beneficiarioIndex == index)
                    indexAlterar = index;

                if (value['CPF'] == cpf && beneficiarioIndex != index) {
                    cpfExistente = true;
                    return false;
                }
            });

            if (cpfExistente) {
                ModalCommons.message("Beneficiário", "CPF já cadastrado");
                return false;
            }

            objAdicionar = {
                "Id": (indexAlterar == null ? 0 : id),
                "CPF": cpf,
                "Nome": nome
            };

            if (indexAlterar != null) {
                beneficiarios[indexAlterar] = objAdicionar;
                ModalCommons.close(idModalData);
            }
            else {
                beneficiarios.push(objAdicionar);
                Beneficiarios.limparCampos();
            }
            
            Beneficiarios.atualizarLista();
        },

        atualizarIndices: function () {
            $.each(beneficiarios, function (index, value) {
                beneficiarios[index]['index'] = index;
            });
        },

        limparCampos: function () {
            beneficiarioIndex = null;
            $("#modalFormBeneficiario #Id").val(0);
            $("#modalFormBeneficiario .classBeneficiario").val("");
            $("#modalFormBeneficiario #CPF").focus();
        },

        remover: function (cpf) {

            $.each(beneficiarios, function (index, value) {
                if (value['CPF'] == cpf) {
                    beneficiarios.splice(index, 1);
                    return false;
                }
            });

            Beneficiarios.atualizarLista();
        },

        atualizarLista: function () {

            Beneficiarios.atualizarIndices();

            $('#gridBeneficiarios').jtable({
                actions: {
                    listAction: function() {
                        return {
                            "Result": "OK",
                            "Records": beneficiarios
                        };
                    }
                },
                fields: {
                    CPF: {
                        title: 'CPF',
                        width: '40%'
                    },
                    Nome: {
                        title: 'Nome',
                        width: '60%'
                    },
                    Alterar: {
                        display: function (data) {
                            return '<button onclick="Beneficiarios.modalDadosBeneficiario(\'' +
                                data.record.Id + '\',\'' +
                                data.record.CPF + '\',\'' +
                                data.record.Nome + '\', ' +
                                data.record.index + ')" class="btn btn-primary btn-sm">Alterar</button>';
                        }
                    },
                    Excluir: {
                        display: function (data) {
                            return '<button onclick="Beneficiarios.remover(\'' + data.record.CPF + '\')" class="btn btn-primary btn-sm">Excluir</button>';
                        }
                    }
                }
            });

            $('#gridBeneficiarios').jtable('load');
        },

        modalBeneficiarios: function (objBeneficiarios = null) {

            if (objBeneficiarios != null)
                beneficiarios = objBeneficiarios;

            let modalCode = '<div class="row"> ' +
                                '<div class="col-md-12"> ' +
                                    '<button type="button" id="btnNovoBeneficiario" class="btn btn-primary btn-sm pull-right">' +
                                        '<span class="glyphicon glyphicon-plus"></span>' +
                                        'Novo' +
                                    '</button>' +
                                '</div > ' +
                            '</div > ' +
                            '<table id="gridBeneficiarios" class="table"></table>';

            ModalCommons.code("Beneficiários", modalCode);

            $("#btnNovoBeneficiario").click(function () {
                Beneficiarios.modalDadosBeneficiario();
            });

            Beneficiarios.atualizarLista();
        },

        modalDadosBeneficiario: function (id = 0, cpf = null, nome = null, index = null) {

            beneficiarioIndex = index;
            let modalCode = '<form id="modalFormBeneficiario" method="post"> ' +
                '<input type="hidden" name="Id" id="Id" value=' + id + '>' +
                '<div class="row"> ' +
                    '<div class="col-md-4"> ' +
                        '<div class="form-group"> ' +
                            '<label for= "CPF"> CPF:</label> ' +
                            '<input required = "required" type = "text" class="form-control cpfMask classBeneficiario" id = "CPF" name = "CPF" placeholder = "Ex.: 010.011.111-00" maxlength = "14"> ' +
                        '</div> ' +
                    '</div> ' +
                    '<div class="col-md-8"> ' +
                        '<div class="form-group"> ' +
                            '<label for= "Nome"> Nome:</label> ' +
                            '<input required = "required" type = "text" class="form-control classBeneficiario" id = "Nome" name = "Nome" placeholder = "Ex.: João da Silva" maxlength = "50"> ' +
                        '</div > ' +
                    '</div > ' + 
                '</div > ' +
                '</form >';

            idModalData = ModalCommons.code("Beneficiários", modalCode, true);

            if (cpf != null) {
                $("#modalFormBeneficiario #Id").val(id);
                $("#modalFormBeneficiario #CPF").val(cpf);
                $("#modalFormBeneficiario #Nome").val(nome);
            }

            $("#" + idModalData + " .btn-confirm").click(function () {
                let form = document.getElementById("modalFormBeneficiario");
                if (!form.checkValidity())
                    return false;

                Beneficiarios.adicionar(
                    $("#modalFormBeneficiario #Id").val(),
                    $("#modalFormBeneficiario #CPF").val(),
                    $("#modalFormBeneficiario #Nome").val()
                );
            });

        }

    }

})(jQuery);

$(function () {
    Beneficiarios.init();
});