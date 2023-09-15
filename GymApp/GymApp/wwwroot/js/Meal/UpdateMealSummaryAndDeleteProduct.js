export function DeleteProduct(jsonData, rowId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: '/Meal/DeleteProduct?jsonData=' + jsonData,
            data: { jsonData: jsonData },
            dataType: 'json',
            success: function () {
                document.getElementById(rowId).remove();
                resolve();
            },
        });
    });
}

export function UpdateMealSummary() {
    $.ajax({
        url: '/Meal/UpdateSummary',
        dataType: 'json',
        success: function (updatedSummary) {
            $('#TotalKcal').text(updatedSummary.totalKcal);
            $('#TotalProtein').text(updatedSummary.totalProtein);
            $('#TotalCarbs').text(updatedSummary.totalCarbs);
            $('#TotalFat').text(updatedSummary.totalFat);
            $('#TotalGrams').text(updatedSummary.totalGrams);
        },
        error: function (innerError) {
            console.error('Inner request error:', innerError);
        }
    });
}

export function EditProduct(jsonData, rowId) {
    $.ajax({
        type:'POST',
        url: '/Meal/EditProduct?jsonData=' + jsonData,
        data: { jsonData: jsonData },
        dataType: 'json',
        success: function () {
            var currentText = "Updated Text";
            var input = $('<input>', {
                type: 'text',
                value: currentText
            });
            var row = document.getElementById(rowId);
            var grams = $(row).find('td:eq(4)');
            grams.html(input);
        }
    })
}