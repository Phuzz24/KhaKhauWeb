document.addEventListener("DOMContentLoaded", function () {
    let slideIndex = 0;
    let slides = document.getElementsByClassName("image-slide");
    let timer;

    // Hiển thị slide đầu tiên khi trang được tải
    showSlides(slideIndex);

    // Điều khiển các nút Previous/Next
    function moveImageSlide(n) {
        clearTimeout(timer); // Xóa bộ đếm giờ khi người dùng click
        slideIndex += n;
        if (slideIndex >= slides.length) slideIndex = 0; // Quay lại slide đầu nếu vượt quá
        if (slideIndex < 0) slideIndex = slides.length - 1; // Quay lại slide cuối nếu nhỏ hơn 0
        showSlides(slideIndex);
    }

    // Hiển thị slide hiện tại
    function showSlides(n) {
        for (let i = 0; i < slides.length; i++) {
            slides[i].style.display = "none"; // Ẩn tất cả slide
        }
        slides[n].style.display = "block"; // Hiển thị slide hiện tại
        timer = setTimeout(function () {
            moveImageSlide(1); // Tự động chuyển slide sau 4 giây
        }, 4000);
    }

    // Gắn sự kiện cho các nút Previous và Next
    document.querySelector(".prev").addEventListener("click", function () {
        moveImageSlide(-1);
    });

    document.querySelector(".next").addEventListener("click", function () {
        moveImageSlide(1);
    });
});


// Thêm hiệu ứng khi người dùng cuộn trang đến các section
$(document).ready(function () {
    $(window).scroll(function () {
        $('.about-section, .mission-section, .gallery-section').each(function () {
            const position = $(this).offset().top;
            const windowTop = $(window).scrollTop();

            if (windowTop > position - $(window).height() + 100) {
                $(this).addClass('fade-in');
            }
        });
    });
});

// Thêm class fade-in để tạo hiệu ứng mờ dần
$('.about-section, .mission-section, .gallery-section').addClass('hidden');

// CSS để ẩn trước khi hiển thị với hiệu ứng
$('.hidden').css({
    opacity: 0,
    transform: 'translateY(20px)',
    transition: 'opacity 0.8s, transform 0.8s',
});

// CSS cho hiệu ứng fade-in
$('.fade-in').css({
    opacity: 1,
    transform: 'translateY(0)',
});


