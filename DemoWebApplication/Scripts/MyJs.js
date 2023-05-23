
function myfunction() {
    alert("Enter Valid Details");
}

function Cancle() {
    document.getElementById('UserName').value = "";
    document.getElementById('Password').value = "";
}

function ClearAll() {
    const element = document.getElementById("myDIV");
    element.getElementsByClassName('form-control')[0].value = "";
    element.getElementsByClassName('form-control')[1].value = "";
    element.getElementsByClassName('form-control')[2].value = "";
    element.getElementsByClassName('form-control')[3].value = "";
    element.getElementsByClassName('form-control')[4].value = "";
};
function ClearAllItem() {
    const element = document.getElementById("myDIV");
    element.getElementsByClassName('form-control')[0].value = "";
    element.getElementsByClassName('form-control')[1].value = "";
    element.getElementsByClassName('form-control')[2].value = "";
    element.getElementsByClassName('form-control')[3].value = "";
    element.getElementsByClassName('form-control')[4].value = "";
};

function CheckValidations(ctrlId, ctrlname) {
    var txtcontrol = document.getElementById("ctrlId");
    var string = document.getElementById("ctrlId").value;
    if (txtcontrol.value ==='') {
        alert('Please Enter ' + ctrlname);
        ctrlname.focus();
        return false;
    }

};


