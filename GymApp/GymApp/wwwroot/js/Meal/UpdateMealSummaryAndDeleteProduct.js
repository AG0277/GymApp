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
            $('#TotalKcal').text(updatedSummary.totalKcal.toFixed(1));
            $('#TotalProtein').text(updatedSummary.totalProtein.toFixed(1));
            $('#TotalCarbs').text(updatedSummary.totalCarbs.toFixed(1));
            $('#TotalFat').text(updatedSummary.totalFat.toFixed(1));
            $('#TotalGrams').text(updatedSummary.totalGrams.toFixed(1));
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

export function EditIcon($EditIconLink) {
    $EditIconLink.on('click', function () {
        var row = $(this).closest('tr');

        var fifthTd = $(row).find('td:eq(5)');
        var currentText = fifthTd.text();
        var columnWidth = '10%';
        fifthTd.css('width', columnWidth);

        var inputElement = $('<input>', {
            type: 'text',
            value: currentText,
            style: 'width:100%'
        });

        fifthTd.html(inputElement);
        inputElement.focus();
        inputElement.keyup(function (event) {
            if (event.key === 'Enter') {
                inputElement.blur();
            }
        });
        inputElement.blur(function () {
            var oldValue = currentText;
            var newValue = inputElement.val();
            var productAttributes = $EditIconLink.data('product-attributes');
            fifthTd.text(newValue);
            EditUpdateProduct(productAttributes, newValue, row,  oldValue)
                .then(function () {
                    UpdateMealSummary();
            })
        });
    });
}

function EditUpdateProduct(productAttributes, newValue, row, oldValue) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: 'POST',
            url: '/Meal/UpdateEditProduct',
            data: JSON.stringify({ productAttributes: productAttributes, newValue: newValue, oldValue: oldValue }),
            contentType: 'application/json',
            dataType: 'json',
            success: function (response) {
                var columnIndexToReplace = [1, 2, 3, 4];
                var newProduct = [response.kcal, response.protein, response.carbs, response.fat, response.grams];
                $.each(columnIndexToReplace, function (index, columnIndex) {
                    row.find('td:eq(' + columnIndex + ')').text(newProduct[index].toFixed(1));
                });
                resolve();
            }
        })
    });
}
