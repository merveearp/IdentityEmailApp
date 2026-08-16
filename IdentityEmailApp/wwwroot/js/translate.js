

    document.addEventListener("DOMContentLoaded", function () {

        const form =
    document.getElementById("translateForm");

    const sourceText =
    document.getElementById("sourceText");

    const translatedText =
    document.getElementById("translatedText");

    const sourceLanguage =
    document.getElementById("sourceLanguage");

    const targetLanguage =
    document.getElementById("targetLanguage");

    const characterCount =
    document.getElementById("characterCount");

    const translateButton =
    document.getElementById("translateButton");

    const translateButtonIcon =
    document.getElementById("translateButtonIcon");

    const translateButtonText =
    document.getElementById("translateButtonText");

    const messageArea =
    document.getElementById("translateMessage");

    const emptyTranslationText =
    "Çeviri burada görüntülenecek...";

    let currentTranslatedText = "";

    // Karakter sayacı
    sourceText.addEventListener("input", function () {
        characterCount.textContent =
        `${sourceText.value.length}/5000`;
        });

    // Mesaj gösterme
    function showMessage(message, type) {
        messageArea.textContent = message;

    messageArea.classList.remove(
    "invisible",
    "bg-red-500/10",
    "text-red-500",
    "border-red-500/20",
    "bg-green-500/10",
    "text-green-600",
    "border-green-500/20"
    );

    if (type === "success") {
        messageArea.classList.add(
            "bg-green-500/10",
            "text-green-600",
            "border-green-500/20"
        );
        }
    else {
        messageArea.classList.add(
            "bg-red-500/10",
            "text-red-500",
            "border-red-500/20"
        );
        }
    }
    function hideMessage() {
        messageArea.classList.add("invisible");
    messageArea.textContent = "";
    }

    // Çeviri işlemi
    form.addEventListener("submit", async function (event) {
        event.preventDefault();

    hideMessage();

    if (!sourceText.value.trim()) {
        showMessage(
            "Lütfen çevrilecek metni giriniz.",
            "error"
        );

    sourceText.focus();
    return;
            }

    translateButton.disabled = true;
    translateButton.classList.add(
    "opacity-60",
    "cursor-not-allowed"
    );

    translateButtonIcon.textContent = "progress_activity";
    translateButtonIcon.classList.add("animate-spin");
    translateButtonText.textContent = "Çevriliyor...";

    try {
                const formData = new FormData(form);

    const response = await fetch(
    '@Url.Action("Translate", "Translate")',
    {
        method: "POST",
    body: formData
                    }
    );

    const result = await response.json();

    if (!result.success) {
        showMessage(result.message, "error");
    return;
                }

    currentTranslatedText =
    result.translatedText ?? "";

    translatedText.textContent =
    currentTranslatedText;

    showMessage(
    "Çeviri başarıyla tamamlandı.",
    "success"
    );
            }
    catch (error) {
        showMessage(
            "Çeviri isteği sırasında bir bağlantı hatası oluştu.",
            "error"
        );
            }
    finally {
        translateButton.disabled = false;

    translateButton.classList.remove(
    "opacity-60",
    "cursor-not-allowed"
    );

    translateButtonIcon.textContent = "translate";
    translateButtonIcon.classList.remove("animate-spin");
    translateButtonText.textContent = "Çevir";
            }
        });

    // Kaynak metni temizle
    document
    .getElementById("clearTextButton")
    .addEventListener("click", function () {

        sourceText.value = "";
    currentTranslatedText = "";

    translatedText.textContent =
    emptyTranslationText;

    characterCount.textContent = "0/5000";

    hideMessage();
    sourceText.focus();
            });

    // Dilleri değiştir
    document
    .getElementById("swapLanguagesButton")
    .addEventListener("click", function () {

                if (sourceLanguage.value === "auto") {
        showMessage(
            "Dilleri değiştirmek için kaynak dili seçiniz.",
            "error"
        );

    return;
                }

    const oldSourceLanguage =
    sourceLanguage.value;

    const oldTargetLanguage =
    targetLanguage.value;

    sourceLanguage.value =
    oldTargetLanguage;

    targetLanguage.value =
    oldSourceLanguage;

    if (currentTranslatedText) {
                    const oldSourceText =
    sourceText.value;

    sourceText.value =
    currentTranslatedText;

    currentTranslatedText =
    oldSourceText;

    translatedText.textContent =
    currentTranslatedText;

    characterCount.textContent =
    `${sourceText.value.length}/5000`;
                }

    hideMessage();
            });

    // Çeviriyi kopyala
    document
    .getElementById("copyTranslationButton")
    .addEventListener("click", async function () {

                if (!currentTranslatedText) {
        showMessage(
            "Kopyalanacak bir çeviri bulunmuyor.",
            "error"
        );

    return;
                }

    try {
        await navigator.clipboard.writeText(
            currentTranslatedText
        );

    showMessage(
    "Çeviri panoya kopyalandı.",
    "success"
    );
                }
    catch {
        showMessage(
            "Çeviri kopyalanamadı.",
            "error"
        );
                }
            });

    // Çeviriyi sesli oku
    document
    .getElementById("listenTranslationButton")
    .addEventListener("click", function () {

                if (!currentTranslatedText) {
        showMessage(
            "Dinlenecek bir çeviri bulunmuyor.",
            "error"
        );

    return;
                }

    window.speechSynthesis.cancel();

    const speech =
    new SpeechSynthesisUtterance(
    currentTranslatedText
    );

    speech.lang = targetLanguage.value;
    window.speechSynthesis.speak(speech);
            });

    // Sesli metin girişi
    document
    .getElementById("voiceInputButton")
    .addEventListener("click", function () {

                const SpeechRecognition =
    window.SpeechRecognition ||
    window.webkitSpeechRecognition;

    if (!SpeechRecognition) {
        showMessage(
            "Tarayıcınız sesli girişi desteklemiyor.",
            "error"
        );

    return;
                }

    const recognition =
    new SpeechRecognition();

    recognition.lang =
    sourceLanguage.value === "auto"
    ? "tr-TR"
    : sourceLanguage.value;

    recognition.onresult = function (event) {
        sourceText.value =
        event.results[0][0].transcript;

    characterCount.textContent =
    `${sourceText.value.length}/5000`;
                };

    recognition.onerror = function () {
        showMessage(
            "Ses algılanırken bir hata oluştu.",
            "error"
        );
                };

    recognition.start();
            });

    });
