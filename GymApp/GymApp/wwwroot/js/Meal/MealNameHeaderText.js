document.addEventListener("DOMContentLoaded", function () {
    var mealName = document.getElementById("MealName");
    var headerText = document.getElementById("headerText");

    mealName.addEventListener("input", function () {
        headerText.innerText = mealName.value;
    });
});