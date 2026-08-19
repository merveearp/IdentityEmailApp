
<script>
    document.addEventListener("DOMContentLoaded", function () {

        // ANTIFORGERY TOKEN
        function getAntiForgeryToken() {
            return $("#notificationForm input[name='__RequestVerificationToken']")
                .val();
        }


        // CONTROLLER'DAN GELEN MESAJI AL
        function getSuccessMessage(response, fallbackMessage) {

            if (typeof response === "string" && response) {
                return response;
            }

    if (response && response.message) {
                return response.message;
            }

    return fallbackMessage;
        }


    // HATA MESAJINI AL
    function getErrorMessage(xhr, fallbackMessage) {

            if (xhr.responseJSON && xhr.responseJSON.message) {
                return xhr.responseJSON.message;
            }

    if (xhr.responseText) {
                return xhr.responseText;
            }

    return fallbackMessage;
        }


    // TÜMÜNÜ SEÇ / SEÇİMLERİ KALDIR
    $("#selectAllButton").on("click", function () {

            const checkboxes =
    $("#notificationForm input[name='ids']");

    const allSelected =
                checkboxes.length > 0 &&
    checkboxes.filter(":checked").length === checkboxes.length;

    if ($.fn.iCheck) {
        checkboxes.iCheck(
            allSelected ? "uncheck" : "check"
        );
            }
    else {
        checkboxes.prop("checked", !allSelected);
            }

    $(this).attr(
    "title",
    allSelected ? "Tümünü Seç" : "Seçimi Kaldır"
    );
        });


    // TOPLU OKUNDU, OKUNMADI VE SİLME
    $(document).on("click", ".bulk-action", function (event) {

        event.preventDefault();

    const button = $(this);
    const actionType = button.data("action");

    const selectedCheckboxes =
    $("#notificationForm input[name='ids']:checked");

    if (selectedCheckboxes.length === 0) {

        Swal.fire({
            title: "Mesaj seçilmedi",
            text: "Lütfen en az bir mesaj seçiniz.",
            icon: "warning",
            confirmButtonText: "Tamam"
        });

    return;
            }


    function sendBulkRequest() {

        $.ajax({
            url: button.data("url"),
            type: "POST",

            // Token ve seçili ids değerlerini gönderir
            data: $("#notificationForm").serialize(),

            success: function (response) {

                const selectedRows =
                    selectedCheckboxes.closest("tr");

                let fallbackMessage =
                    "İşlem başarıyla tamamlandı.";


                // OKUNDU
                if (actionType === "read") {

                    selectedRows
                        .removeClass("unread-message")
                        .addClass("read-message");

                    fallbackMessage =
                        "Seçilen mesajlar okundu olarak işaretlendi.";
                }


                // OKUNMADI
                if (actionType === "unread") {

                    selectedRows
                        .removeClass("read-message")
                        .addClass("unread-message");

                    fallbackMessage =
                        "Seçilen mesajlar okunmadı olarak işaretlendi.";
                }


                // ÇÖP KUTUSUNA TAŞIMA
                if (actionType === "delete") {

                    selectedRows.fadeOut(200, function () {
                        $(this).remove();
                    });

                    fallbackMessage =
                        "Seçilen mesajlar çöp kutusuna taşındı.";
                }
                else {

                    // Okundu/okunmadı işleminden sonra seçimleri kaldır
                    if ($.fn.iCheck) {
                        selectedCheckboxes.iCheck("uncheck");
                    }
                    else {
                        selectedCheckboxes.prop("checked", false);
                    }
                }

                $("#selectAllButton")
                    .attr("title", "Tümünü Seç");

                Swal.fire({
                    title: "Başarılı",
                    text: getSuccessMessage(
                        response,
                        fallbackMessage
                    ),
                    icon: "success",
                    timer: 1500,
                    showConfirmButton: false
                });
            },

            error: function (xhr) {

                console.log(
                    "Durum kodu:",
                    xhr.status
                );

                console.log(
                    "Sunucu cevabı:",
                    xhr.responseText
                );

                Swal.fire({
                    title: "Hata",
                    text: getErrorMessage(
                        xhr,
                        "Toplu işlem sırasında bir hata oluştu."
                    ),
                    icon: "error",
                    confirmButtonText: "Tamam"
                });
            }
        });
            }


    // TOPLU SİLME ONAYI
    if (actionType === "delete") {

        Swal.fire({
            title: "Seçilen mesajlar silinsin mi?",
            text: "Mesajlar çöp kutusuna taşınacak.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Evet, sil",
            cancelButtonText: "Vazgeç",
            confirmButtonColor: "#d33",
            cancelButtonColor: "#6c757d"
        }).then(function (result) {

            if (result.isConfirmed) {
                sendBulkRequest();
            }
        });

    return;
            }

    sendBulkRequest();
        });


    // TEKLİ YILDIZ, SPAM VE ÇÖP KUTUSU İŞLEMLERİ
    $(document).on("click", ".mail-action", function (event) {

        event.preventDefault();

    const button = $(this);
    const icon = button.find("i");

    $.ajax({
        url: button.data("url"),
    type: "POST",

    data: {
        id: button.data("id"),
    __RequestVerificationToken:
    getAntiForgeryToken()
                },

    success: function () {

                    // YILDIZLAMA
                    if (button.hasClass("mail-star")) {

                        if (icon.hasClass("fa-star")) {

        icon
            .removeClass("fa-star starred")
            .addClass("fa-star-o");

    button.attr(
    "title",
    "Yıldızla"
    );
                        }
    else {

        icon
            .removeClass("fa-star-o")
            .addClass("fa-star starred");

    button.attr(
    "title",
    "Yıldızı Kaldır"
    );
                        }
                    }


    // SPAM VEYA ÇÖP KUTUSU
    if (
    button.hasClass("mail-spam") ||
    button.hasClass("mail-trash")
    ) {
        button.closest("tr").fadeOut(
            200,
            function () {
                $(this).remove();
            }
        );
                    }
                },

    error: function (xhr) {

        console.log(
            "Durum kodu:",
            xhr.status
        );

    console.log(
    "Sunucu cevabı:",
    xhr.responseText
    );

    Swal.fire({
        title: "Hata",
    text: getErrorMessage(
    xhr,
    "İşlem sırasında bir hata oluştu."
    ),
    icon: "error",
    confirmButtonText: "Tamam"
                    });
                }
            });
        });


    $(document).on("click", ".mail-permanent-delete", function (event) {

        event.preventDefault();

    const button = $(this);
    const token = getAntiForgeryToken();

    if (!token) {
        console.log("Antiforgery token bulunamadı.");

    Swal.fire({
        title: "Hata",
    text: "Güvenlik anahtarı bulunamadı.",
    icon: "error"
        });

    return;
    }

    Swal.fire({
        title: "Mesaj kalıcı olarak silinsin mi?",
    text: "Bu işlem geri alınamaz.",
    icon: "warning",
    showCancelButton: true,
    confirmButtonText: "Evet, sil",
    cancelButtonText: "Vazgeç",
    confirmButtonColor: "#d33",
    cancelButtonColor: "#6c757d"
    }).then(function (result) {

        if (!result.isConfirmed) {
            return;
        }

    $.ajax({
        url: button.data("url"),
    type: "POST",

    data: {
        id: button.data("id"),
    __RequestVerificationToken: token
            },

    success: function (response) {

        button.closest("tr").fadeOut(200, function () {
            $(this).remove();
        });

    const message =
    response && response.message
    ? response.message
    : "Mesaj kalıcı olarak silindi.";

    Swal.fire({
        title: "Silindi",
    text: message,
    icon: "success",
    timer: 1500,
    showConfirmButton: false
                });
            },

    error: function (xhr) {

        console.log("Durum kodu:", xhr.status);
    console.log("Sunucu cevabı:", xhr.responseText);

    const message =
    xhr.responseJSON?.message ||
    xhr.responseText ||
    "Mesaj kalıcı olarak silinemedi.";

    Swal.fire({
        title: "Hata",
    text: message,
    icon: "error",
    confirmButtonText: "Tamam"
                });
            }
            });
        });
    });

});
</script>