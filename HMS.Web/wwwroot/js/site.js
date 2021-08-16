$(function () {
    $('input[name="dateRange"]').daterangepicker();
});
$(function () {
    $('input[name="dateRange2"]').daterangepicker();
});

$(function () {
    $('input[name="outDay"]').daterangepicker({
        singleDatePicker: true
    });
});
$(function () {
    $('#periodCheck').on('click', function () {
        if ($(this).is(':checked')) {
            //$('#inputdaterangeUser').disabled = true;
            document.getElementById("inputdaterangeUser").disabled = true;
        }
    });
});
$('#periodCheck2').on('click', function () {
    if ($(this).is(':checked')) {
        $('#inputdaterangeUser2').disabled = true;
    }
});


let servicesList = document.querySelectorAll(".add-service-transaction-button");
for (let service of servicesList) {
    service.addEventListener("click", AddServiceTransaction)
};

function AddServiceTransaction(event) {
    //let transactionList = updateTransaction(event.target.getAttribute("value"));
    let select = document.getElementById("add-service-transaction-select");

    let form, input, label, submitBtn, cancelBtn, element, coverDiv, labelForTransaction;
    element = document.getElementById("add-service-transaction-container");
    if (input) input.remove();
    let el = document.getElementById("add-service-transaction-select__reservation");
    if (el) {
        el.remove();
    }
    let selectTransaction = document.createElement('select');
    selectTransaction.className = "form-control";
    selectTransaction.setAttribute("id", "add-service-transaction-select__reservation")
    selectTransaction.setAttribute("name", "userTransaction");
    //console.log(transactionList);
    $.when(updateTransaction(event.target.getAttribute("value"))).done(function (data) {
        console.log("lalala");
        for (let transaction of data) {
            let option = document.createElement("option");
            option.setAttribute("value", transaction);
            option.textContent = transaction;
            selectTransaction.append(option);
        }
    });


    if (!document.body.querySelector(".add-service-transaction-form")) {
        select.remove();
        select.style.display = "block";
        select.style.marginBottom = "10px";

        form = document.createElement("form");
        form.setAttribute("action", "https://localhost:44352/Admin/AddService")
        form.setAttribute("method", "post");
        form.className = "add-service-transaction-form";
        form.style.marginTop = "100px";

        label = document.createElement("label");
        label.setAttribute("for", "add-service-transaction-select");
        label.textContent = "Выберете предоставленные клиенту услуги";

        labelForTransaction = document.createElement("label");
        labelForTransaction.setAttribute("for", "add-service-transaction-select__reservation");
        labelForTransaction.textContent = "Выберете бронирование для которого предостовлялись услуги";

        submitBtn = document.createElement("input");
        submitBtn.setAttribute("name", "submit");
        submitBtn.setAttribute("type", "submit");
        submitBtn.textContent = "Добавить услуги";
        submitBtn.className = "btn";
        submitBtn.classList.add("btn-dark");

        cancelBtn = document.createElement("button");
        cancelBtn.setAttribute("type", "button");
        cancelBtn.setAttribute("name", "cancel");
        cancelBtn.textContent = "Отмена";
        cancelBtn.className = "btn";
        cancelBtn.classList.add("btn-dark");
        cancelBtn.style.marginLeft = "15px";

        element.append(form);
        form.append(labelForTransaction);
        form.append(selectTransaction);
        form.append(label);
        form.append(select);
        form.append(submitBtn);
        form.append(cancelBtn);


    }
    else {
        form = document.body.querySelector(".add-service-transaction-form");
        labelForTransaction = form.querySelector("label");
        labelForTransaction.after(selectTransaction);
        submitBtn = form.querySelector('input:first-child');
        cancelBtn = form.querySelector('button:last-child');

    }

    coverDiv = document.createElement("div");
    coverDiv.className = 'add-service-transaction-cover';

    element.className = "add-service-transaction-container";

    select.setAttribute("name", "serviceList");
    document.body.style.overflowY = "hidden";


    let targetId = event.target.getAttribute("value");
    document.body.querySelector("main").append(coverDiv);
    element.style.display = 'block';

    form.addEventListener("submit", function () {
        input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", "userId");
        input.setAttribute("value", targetId);
        form.append(input);
        coverDiv.remove();
        element.style.display = "none";
    });

    cancelBtn.addEventListener("click", function () {
        coverDiv.remove();
        element.style.display = "none";
    });
    document.addEventListener("keydown", function () {
        if (event.key == "Escape" && coverDiv) {
            coverDiv.remove();
            element.style.display = "none";
        }
    });
    function updateTransaction(userId) {
        let transactionList = null;
        $.ajaxSetup({ cache: false });
        return $.ajax({
            type: "GET",
            url: "https://localhost:44352/Admin/GetUserTransaction",
            cache: false,
            data: {
                userId: userId
            },
            dataType: "json"
            //success: function (data) {
            //    console.log( data);
            //    transactionList = data;
            //},
            //error: function () {
            //    console.log("error ajax");
            //}
        });

        //return  transactionList;
    }
}




let servicesPaymentBtnList = document.querySelectorAll(".service-payment-button");
for (let servicePaymentBtn of servicesPaymentBtnList) {
    servicePaymentBtn.addEventListener("click", AddServicePayment);
}

function AddServicePayment(event) {
    let target = event.target;

    let formBlock = document.createElement("div");
    formBlock.className = "add-service-transaction-container";

    let coverDiv = document.createElement("div");
    coverDiv.className = "add-service-transaction-cover";

    let form = document.createElement("form");
    form.setAttribute("action", "https://localhost:44352/Reservation/ServicePayment");
    form.setAttribute("method", "POST");
    form.className = "add-service-transaction-form";
    form.style.marginTop = "250px";

    let labelCardNumber = document.createElement("label");
    labelCardNumber.textContent = "Номер карты";
    labelCardNumber.setAttribute("for", "card-number");

    let inputCardNumber = document.createElement("input");
    inputCardNumber.setAttribute("type", "text");
    inputCardNumber.setAttribute("id", "card-number");
    inputCardNumber.className = "form-control";

    let labelCardOwner = document.createElement("label");
    labelCardOwner.setAttribute("for", "card-owner-name");
    labelCardOwner.textContent = "Владелец карты";

    let inputCardOwner = document.createElement("input");
    inputCardOwner.setAttribute("type", "text");
    inputCardOwner.setAttribute("id", "card-owner-name");
    inputCardOwner.className = "form-control";


    let submitBtn = document.createElement("input");
    submitBtn.classList.add("btn", "btn-dark");
    submitBtn.setAttribute("type", "submit");
    submitBtn.setAttribute("value", "Оплатить");

    let cancelBtn = document.createElement("button");
    cancelBtn.classList.add("btn", "btn-dark");
    cancelBtn.textContent = "Отмена";


    form.append(labelCardOwner);
    form.append(inputCardOwner);
    form.append(labelCardNumber);
    form.append(inputCardNumber);
    form.append(submitBtn);
    form.append(cancelBtn);
    formBlock.append(form);
    document.body.querySelector("main").append(formBlock);
    document.body.querySelector("main").append(coverDiv);


    document.body.style.overflowY = "hidden";
    formBlock.style.display = "block";

    let transactionData = event.target.dataset.transactionid;

    form.onsubmit = function () {
        return false;
    }

    submitBtn.addEventListener("click", function () {
        event.preventDefault();
        let transaction = transactionData;
        let paymentId = null;
        function paymentPOST() {
            return $.ajax({
                type: "POST",
                cache: false,
                dataType: "text",
                url: "https://localhost:44352/Reservation/ServicePayment",
                data: {
                    transactionId: transaction,
                    cardOwner: inputCardOwner.value,
                    cardNumber: inputCardNumber.value
                }
                //success: function (data) {
                //    console.log("Payment - OK");
                //    paymentId = data;
                //},
                //error: function () {
                //    console.log("Payment - ERROR");
                //}
                //complete: function () {
                //    target.closest("tr").querySelector("td:nth-of-type(3)").textContent = paymentId;
                //    target.closest("td").textContent = "Payed";
                //    //formBlock.remove();
                //    //coverDiv.remove();
                //}
            });
        }
        $.when(paymentPOST()).done(function (data) {
            target.closest("tr").querySelector("td:nth-of-type(3)").textContent = data;
            target.closest("td").textContent = "Payed";
            formBlock.remove();
            coverDiv.remove();
            document.body.style.overflowY = ''
        });

    });

    cancelBtn.addEventListener("click", function () {
        formBlock.remove();
        coverDiv.remove();
        document.body.style.overflowY = ''
    });

    document.addEventListener("keydown", function () {
        if (event.key == "Escape" && formBlock) {
            formBlock.remove();
            coverDiv.remove();
            document.body.style.overflowY = ''
        }
    });

}


let slider = document.getElementById("info-slider");
if (slider != null) {
    autoSlider();
    function autoSlider() {
        let timer = setInterval(sliderChangeItem, 10000);
    }

    function sliderChangeItem() {
        let btnItem = slider.querySelector("input[checked]");
        let index = parseInt(btnItem.dataset.index, 10);
        let nextIndex = index + 1 == 5 ? 1 : index + 1;
        let buttons = slider.querySelectorAll("input");
        for (let button of buttons) {
            if (parseInt(button.dataset.index, 10) === nextIndex) {
                btnItem.removeAttribute("checked");
                button.setAttribute("checked", true);
                break;
            }
        }
    }
}
