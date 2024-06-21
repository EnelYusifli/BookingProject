let deleteBtns = document.querySelectorAll(".delete-btn");

deleteBtns.forEach(btn => {
    let url = btn.getAttribute("href")
    btn.addEventListener("click", function (e) {
        e.preventDefault();
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(response => {
                        if (response.status == 200) {
                            window.location.reload(true);
                        } else alert("Cannot be deleted")
                    })
            }
        });
    })
})
let reportBtns = document.querySelectorAll(".report-btn");

reportBtns.forEach(btn => {
    let url = btn.getAttribute("href")
    btn.addEventListener("click", function (e) {
        e.preventDefault();
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, report it!"
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(response => {
                        if (response.status == 200) {
                            window.location.reload(true);
                        } else alert("Cannot be reported")
                    })
            }
        });
    })
})

let softDeleteBtns = document.querySelectorAll(".hotel-softdelete-btn");

softDeleteBtns.forEach(btn => {
    let url = btn.getAttribute("href");
    btn.addEventListener("click", function (e) {
        e.preventDefault();
        Swal.fire({
            title: "Are you sure?",
            text: "This will only mark the hotel as deleted.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, mark as deleted!"
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(response => {
                        if (response.status == 200) {
                            window.location.reload(true);
                        } else alert("Cannot be marked as deleted");
                    })
            }
        });
    });
});