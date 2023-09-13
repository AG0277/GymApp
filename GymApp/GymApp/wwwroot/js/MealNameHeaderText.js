document.addEventListener("DOMContentLoaded", function () {
    var mealName = document.getElementById("mealName");
    var headerText = document.getElementById("headerText");

    mealName.addEventListener("input", function () {
        headerText.innerText = mealName.value;
    });
});