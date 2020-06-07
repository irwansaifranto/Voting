(function ($) {
    'use strict';

    var initialCategories = (function () {
        let urlContext = '';
        let gridListCategories, btnAddCat, modalCatForm, btnSubmitChanges,
            votCatId, votCatName, errMsgVotCatName;

        var initialDom = function () {
            gridListCategories = $('#grid-list-category');
            btnAddCat = $('#btn-add-category');
            modalCatForm = $('#addCategoriesModal');
            btnSubmitChanges = $('#btnSubmitChanges');
            votCatId = $('#VotingCategoryId');
            votCatName = $('#VotingCategoryName');
            errMsgVotCatName = $('#errMsgVotCatName');
        };

        var initialgridListCategories = function () {
            gridListCategories.DataTable({
                ajax: { url: WEBPORTAL.URLContext.GetCategories, dataSrc: '' },
                //searching: false,
                info: false,
                lengthChange: false,
                //paging: false,
                columns: [
                    { data: "votingCategoryId", title: "", visible: false },
                    { data: "votingCategoryName", title: "Category Name" },
                    { data: "createdBy", title: "Created By" },
                    {
                        data: null,
                        title: "",
                        width: 60,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html('<div class="btn-group">'
                                + '<a href="#" class="btn btn-icon waves-effect waves-light btn-custom bp_tbl_btn_edit"> <i class="fa fa-edit"></i></a>'
                                + '<a href="#" class="btn btn-icon waves-effect waves-light btn-outline-custom bp_tbl_btn_delete"><i class="fa fa-trash"></i></a>'
                                + '</div>');

                            $(td).find('.bp_tbl_btn_edit').click(function () {
                                modalCatForm.find('.modal-dialog .modal-content .modal-title').text('Update Category');
                                btnSubmitChanges.text("Update");

                                // set value
                                urlContext = WEBPORTAL.URLContext.UpdateCategory;

                                votCatId.val(rowData.votingCategoryId);
                                votCatName.val(rowData.votingCategoryName);

                                modalCatForm.modal('show');
                            });

                            $(td).find('.bp_tbl_btn_delete').click(function () {
                                const swalWithBootstrapButtons = Swal.mixin({
                                    customClass: {
                                        confirmButton: 'btn btn-danger',
                                        cancelButton: 'btn btn-default'
                                    },
                                    buttonsStyling: false
                                });

                                swalWithBootstrapButtons.fire({
                                    title: 'Are you sure?',
                                    text: "You won't be able to revert this!",
                                    type: 'warning',
                                    showCancelButton: true,
                                    confirmButtonText: 'Yes, delete it!',
                                    cancelButtonText: 'No, cancel!',
                                    reverseButtons: true
                                }).then((result) => {
                                    if (result.value) {
                                        $.when(WEBPORTAL.Services.POSTLocal(null, WEBPORTAL.URLContext.DeleteCategory + '?id=' + rowData.votingCategoryId)).done(function (result, status, xhr) {
                                            reloadDataSource();
                                            swalWithBootstrapButtons.fire(
                                                'Deleted!',
                                                'Your file has been deleted.',
                                                'success'
                                            )
                                        });
                                    } else if (result.dismiss === Swal.DismissReason.cancel) {
                                        swalWithBootstrapButtons.fire(
                                            'Cancelled',
                                            'Your imaginary data is safe :)',
                                            'warning'
                                        )
                                    }
                                });
                            });
                        }
                    }
                ]                
            });         
        };

        var reloadDataSource = function () {
            gridListCategories.DataTable().ajax.reload();
        };

        var openCatModalForm = function () {
            btnAddCat.click(function () {
                modalCatForm.find('.modal-dialog .modal-content .modal-title').text('Create new category');
                btnSubmitChanges.text("Save");
                modalCatForm.modal('show');
                resetVoteModalForm();
                urlContext = WEBPORTAL.URLContext.InsertCategory;
            });
        };

        var submitFormChanges = function () {
            btnSubmitChanges.click(function () {
                WEBPORTAL.Utility.SubmitLoading(btnSubmitChanges);
                if (formValidation()) {
                    var model = new FormData();

                    model.append('VotingCategoryId', votCatId.val());
                    model.append('VotingCategoryName', votCatName.val());

                    $.when(WEBPORTAL.Services.POSTLocal(model, urlContext, false, false)).done(function (result, status, xhr) {
                        if (result.code === 200) {
                            setTimeout(function () {
                                $.when(modalCatForm.modal('hide')).then(resetVoteModalForm());
                                WEBPORTAL.Utility.SubmitRemoveLoading(btnSubmitChanges);
                                WEBPORTAL.Utility.ConstructNotificationSuccess(result.message);
                                reloadDataSource();
                            }, 500);
                        } else {
                            setTimeout(function () {
                                modalCatForm.modal('hide');
                                WEBPORTAL.Utility.SubmitRemoveLoading(btnSubmitChanges);
                                WEBPORTAL.Utility.ConstructNotificationError(result.message);
                            }, 500);
                        }
                    });
                } else {
                    setTimeout(function () {
                        WEBPORTAL.Utility.SubmitRemoveLoading(btnSubmitChanges);
                    }, 500);
                }
            });
        };

        var formValidation = function () {
            let isVotCatName = false, isValid = false;

            // voting Category Name
            if (votCatName.val() === '' || votCatName.val() === null) {
                errMsgVotCatName.text('Field is required');

                isVotCatName = false;
            } else {
                errMsgVotCatName.text('');

                isVotCatName = true;
            }

            if (isVotCatName) {
                isValid = true;
            }

            return isValid;
        };

        var resetVoteModalForm = function () {
            votCatName.val('');
        };

        var run = function () {
            initialDom();
            initialgridListCategories();
            openCatModalForm();
            submitFormChanges();
        };

        return {
            run: run
        };
    })();

    initialCategories.run();
})(jQuery);