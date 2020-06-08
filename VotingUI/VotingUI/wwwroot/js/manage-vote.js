(function ($) {
    'use strict';

    var initialVoting = (function () {
        let urlContext = '';
        let gridListVote, btnAddVote, modalVoteForm, btnSubmitChanges,
            votProcId, votProcName, description, createdDate, dueDate, votingCategoryId,
            errMsgVotProcName, errMsgDesc, errMsgCreatedDate, errMsgDueDate, errMsgCategory;

        var initialDom = function () {
            gridListVote = $('#grid-list-voting');
            btnAddVote = $('#btn-add-vote');
            modalVoteForm = $('#addVotingModal');
            btnSubmitChanges = $('#btnSubmitChanges');
            votProcId = $('#VotingProcessId');
            votProcName = $('#VotingProcessName');
            description = $('#Description');
            createdDate = $('#CreatedDate');
            dueDate = $('#DueDate');
            votingCategoryId = $('#VotingCategoryId');
            errMsgVotProcName = $('#errMsgVotProcName');
            errMsgDesc = $('#errMsgDesc');
            errMsgCreatedDate = $('#errMsgCreatedDate');
            errMsgDueDate = $('#errMsgDueDate');
            errMsgCategory = $('#errMsgCategory');
        };

        var initialgridListVote = function () {
            gridListVote.DataTable({
                ajax: { url: WEBPORTAL.URLContext.GetVotings, dataSrc: '' },
                //searching: false,
                info: false,
                lengthChange: false,
                //paging: false,
                columns: [
                    { data: "votingProcessId", title: "", visible: false },
                    { data: "votingProcessName", title: "Name" },
                    { data: "description", title: "Description" },
                    { data: "stringCreatedDate", title: "Open Voting" },
                    {
                        data: "reviewers",
                        title: "Reviewers",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (cellData === null)
                                $(td).html('<div><span class="badge badge-primary" style="background-color: #007bff;">' + 0 + '</span> reviews <i class="fa fa-eye" aria-hidden="true"></i></div>');
                            else
                                $(td).html('<div><span class="badge badge-primary" style="background-color: #007bff;">' + cellData + '</span> reviews <i class="fa fa-eye" aria-hidden="true"></i></div>');
                        }
                    },
                    { data: "stringDueDate", title: "Closed Voting" },
                    { data: "votingCategoryId", title: "", visible: false },
                    { data: "votingCategoryName", title: "Category" },
                    //{ data: null, title: "Ratings" },
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
                                if ($('#role') == 'Admin') {
                                    modalVoteForm.find('.modal-dialog .modal-content .modal-title').text('Update Vote');
                                    btnSubmitChanges.text("Update");

                                    // set value
                                    urlContext = WEBPORTAL.URLContext.UpdateVoting;

                                    votProcId.val(rowData.votingProcessId);
                                    votProcName.val(rowData.votingProcessName);
                                    description.val(rowData.description);
                                    createdDate.val(WEBPORTAL.Utility.ConvertDateStringToJavascript(rowData.stringCreatedDate));
                                    dueDate.val(WEBPORTAL.Utility.ConvertDateStringToJavascript(rowData.stringDueDate));
                                    votingCategoryId.val(rowData.votingCategoryId);

                                    modalVoteForm.modal('show');
                                } else {
                                    WEBPORTAL.Utility.ConstructNotificationError("You haven't access to perform this action.");
                                }
                            });

                            $(td).find('.bp_tbl_btn_delete').click(function () {
                                if ($('#role') == 'Admin') {
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
                                            $.when(WEBPORTAL.Services.POSTLocal(null, WEBPORTAL.URLContext.DeleteVoting + '?votingProcessId=' + rowData.votingProcessId)).done(function (result, status, xhr) {
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
                                } else {
                                    WEBPORTAL.Utility.ConstructNotificationError("You haven't access to perform this action.");
                                }                                
                            });
                        }
                    }
                ],
                columnDefs: [
                    {
                        orderable: false,
                        targets: [8]
                    },
                    {
                        searchable: false,
                        targets: [2,3,4,5,6,8]
                    }
                ],
                initComplete: function () {
                    this.api().columns().every(function () {
                        var column = this;
                        if (column[0][0] == 7) {
                            var select = $('<select class="form-control" id="category-filter" style="width: 50%"><option value="">Select</option></select>')
                                .appendTo($('#grid-list-voting_filter').parent().siblings())
                                .on('change', function () {
                                    var val = $.fn.dataTable.util.escapeRegex(
                                        $(this).val()
                                    );

                                    column
                                        .search(val ? '^' + val + '$' : '', true, false)
                                        .draw();
                                });

                            column.data().unique().sort().each(function (d, j) {
                                select.append('<option value="' + d + '">' + d + '</option>')
                            });
                        }
                        
                    });
                }
            });

            //$('#grid-list-voting_filter').parent().siblings().html('<select class="form-control" id="category-filter" style="width: 50%"><option>Select</option></select>');
        };

        var reloadDataSource = function () {
            gridListVote.DataTable().ajax.reload();
        };

        var openVotingModalForm = function () {
            btnAddVote.click(function () {
                modalVoteForm.find('.modal-dialog .modal-content .modal-title').text('Create new vote');
                btnSubmitChanges.text("Save");
                modalVoteForm.modal('show');
                resetVoteModalForm();
                urlContext = WEBPORTAL.URLContext.InsertVoting;
            });
        };

        var submitFormChanges = function () {
            btnSubmitChanges.click(function () {
                WEBPORTAL.Utility.SubmitLoading(btnSubmitChanges);
                if (formValidation()) {
                    var model = new FormData();

                    model.append('VotingProcessId', votProcId.val());
                    model.append('VotingProcessName', votProcName.val());
                    model.append('Description', description.val());
                    model.append('CreatedDate', createdDate.val());
                    model.append('DueDate', dueDate.val());
                    model.append('VotingCategoryId', votingCategoryId.val());

                    $.when(WEBPORTAL.Services.POSTLocal(model, urlContext, false, false)).done(function (result, status, xhr) {
                        if (result.code === 200) {
                            setTimeout(function () {
                                $.when(modalVoteForm.modal('hide')).then(resetVoteModalForm());
                                WEBPORTAL.Utility.SubmitRemoveLoading(btnSubmitChanges);
                                WEBPORTAL.Utility.ConstructNotificationSuccess(result.message);
                                reloadDataSource();
                            }, 500);
                        } else {
                            setTimeout(function () {
                                modalVoteForm.modal('hide');
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
            let isVotProcName = false, isDesc = false, isCategory = false, isValid = false;

            // Name
            if (votProcName.val() === '' || votProcName.val() === null) {
                errMsgVotProcName.text('Field is required');

                isVotProcName = false;
            } else {
                errMsgVotProcName.text('');

                isVotProcName = true;
            }

            // Description
            if (description.val() === '' || description.val() === null) {
                errMsgDesc.text('Field is required');

                isDesc = false;

            } else {
                errMsgDesc.text('');

                isDesc = true;
            }

            // Category
            if (votingCategoryId.val() === '' || votingCategoryId.val() === null) {
                errMsgCategory.text('Field is required');

                isCategory = false;

            } else {
                errMsgCategory.text('');

                isCategory = true;
            }

            if (isVotProcName && isDesc && isCategory) {
                isValid = true;
            }

            return isValid;
        };

        var resetVoteModalForm = function () {
            votProcName.val('');
            description.val('');
            createdDate.val(getToday());
            dueDate.val(getToday());
            votingCategoryId.val('');
        };

        var getToday = function () {
            var now = new Date();

            var day = ("0" + now.getDate()).slice(-2);
            var month = ("0" + (now.getMonth() + 1)).slice(-2);
            var today = now.getFullYear() + "-" + (month) + "-" + (day);

            return today;
        };

        var getTomorrow = function () {
            var now = new Date();

            var day = ("0" + now.getDate()).slice(-2);
            var month = ("0" + (now.getMonth() + 1)).slice(-2);
            var today = now.getFullYear() + "-" + (month) + "-" + (day);

            return today;
        };

        var run = function () {
            initialDom();
            initialgridListVote();
            openVotingModalForm();
            submitFormChanges();
        };

        return {
            run: run
        };
    })();

    initialVoting.run();
})(jQuery);