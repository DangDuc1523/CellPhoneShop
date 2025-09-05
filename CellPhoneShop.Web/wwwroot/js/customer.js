// Customer Interface JavaScript

document.addEventListener("DOMContentLoaded", function () {
    // Initialize tooltips
    const tooltipTriggerList = [].slice.call(
        document.querySelectorAll('[data-bs-toggle="tooltip"]')
    );
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize popovers
    const popoverTriggerList = [].slice.call(
        document.querySelectorAll('[data-bs-toggle="popover"]')
    );
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Product search functionality
    const searchInput = document.getElementById("productSearch");
    if (searchInput) {
        searchInput.addEventListener("input", function () {
            const searchTerm = this.value.toLowerCase();
            const productCards = document.querySelectorAll(".product-card");

            productCards.forEach(function (card) {
                const productName = card
                    .querySelector(".card-title")
                    .textContent.toLowerCase();
                const productBrand = card
                    .querySelector(".brand")
                    .textContent.toLowerCase();

                if (
                    productName.includes(searchTerm) ||
                    productBrand.includes(searchTerm)
                ) {
                    card.style.display = "";
                    card.classList.add("fade-in");
                } else {
                    card.style.display = "none";
                }
            });
        });
    }

    // Add to cart functionality
    const addToCartButtons = document.querySelectorAll(".add-to-cart-btn");
    addToCartButtons.forEach(function (button) {
        button.addEventListener("click", function (e) {
            e.preventDefault();

            const productId = this.getAttribute("data-product-id");
            const productName = this.getAttribute("data-product-name");

            addToCart(productId, productName);
        });
    });

    // Add to wishlist functionality (newly added button on Index.cshtml)
    const addToWishlistButtons = document.querySelectorAll(
        ".add-to-wishlist-btn"
    );
    addToWishlistButtons.forEach(function (button) {
        button.addEventListener("click", function (e) {
            e.preventDefault();
            const productId = this.getAttribute("data-product-id");
            // Pass the button element itself to the function
            toggleWishlist(productId, this);
        });
    });

    // Quantity controls
    const quantityControls = document.querySelectorAll(".quantity-control");
    quantityControls.forEach(function (control) {
        const minusBtn = control.querySelector(".quantity-minus");
        const plusBtn = control.querySelector(".quantity-plus");
        const quantityInput = control.querySelector(".quantity-input");

        if (minusBtn) {
            minusBtn.addEventListener("click", function () {
                let currentValue = parseInt(quantityInput.value);
                if (currentValue > 1) {
                    quantityInput.value = currentValue - 1;
                }
            });
        }

        if (plusBtn) {
            plusBtn.addEventListener("click", function () {
                let currentValue = parseInt(quantityInput.value);
                quantityInput.value = currentValue + 1;
            });
        }
    });

    // Filter functionality
    const filterCheckboxes = document.querySelectorAll(".filter-checkbox");
    filterCheckboxes.forEach(function (checkbox) {
        checkbox.addEventListener("change", function () {
            applyFilters();
        });
    });

    // Price range slider
    const priceRangeSlider = document.getElementById("priceRange");
    if (priceRangeSlider) {
        priceRangeSlider.addEventListener("input", function () {
            const priceValue = document.getElementById("priceValue");
            if (priceValue) {
                priceValue.textContent = "$" + this.value;
            }
        });
    }

    // Wishlist functionality
    const wishlistButtons = document.querySelectorAll(".wishlist-btn");
    wishlistButtons.forEach(function (button) {
        button.addEventListener("click", function (e) {
            e.preventDefault();

            const productId = this.getAttribute("data-product-id");
            toggleWishlist(productId, this);
        });
    });

    // Product image gallery
    const productImages = document.querySelectorAll(".product-thumbnail");
    const mainImage = document.getElementById("mainProductImage");

    if (productImages.length > 0 && mainImage) {
        productImages.forEach(function (thumbnail) {
            thumbnail.addEventListener("click", function () {
                const imageSrc = this.getAttribute("data-image");
                mainImage.src = imageSrc;

                // Remove active class from all thumbnails
                productImages.forEach((img) => img.classList.remove("active"));
                // Add active class to clicked thumbnail
                this.classList.add("active");
            });
        });
    }

    // Review form
    const reviewForm = document.getElementById("reviewForm");
    if (reviewForm) {
        reviewForm.addEventListener("submit", function (e) {
            e.preventDefault();
            submitReview(this);
        });
    }

    // Newsletter subscription
    const newsletterForm = document.getElementById("newsletterForm");
    if (newsletterForm) {
        newsletterForm.addEventListener("submit", function (e) {
            e.preventDefault();
            subscribeNewsletter(this);
        });
    }

    // Smooth scrolling for anchor links
    const anchorLinks = document.querySelectorAll('a[href^="#"]');
    anchorLinks.forEach(function (link) {
        link.addEventListener("click", function (e) {
            e.preventDefault();
            const targetId = this.getAttribute("href");
            const targetElement = document.querySelector(targetId);

            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: "smooth",
                    block: "start",
                });
            }
        });
    });

    // Lazy loading for images
    const lazyImages = document.querySelectorAll("img[data-src]");
    if ("IntersectionObserver" in window) {
        const imageObserver = new IntersectionObserver(function (
            entries,
            observer
        ) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.getAttribute("data-src");
                    img.classList.remove("lazy");
                    imageObserver.unobserve(img);
                }
            });
        });

        lazyImages.forEach(function (img) {
            imageObserver.observe(img);
        });
    }

    // Product Quick View functionality
    const productQuickViewModal = new bootstrap.Modal(
        document.getElementById("productQuickViewModal")
    );

    document.querySelectorAll(".quick-view-btn").forEach((button) => {
        button.addEventListener("click", function () {
            const productId = this.getAttribute("data-product-id");
            // Simulate fetching product data (replace with actual API call later)
            const productData = getHardcodedProductDetails(productId);

            if (productData) {
                document.getElementById("quickViewProductImage").src =
                    productData.imageUrl;
                document.getElementById("quickViewProductName").textContent =
                    productData.phoneName;
                document.getElementById("quickViewProductBrand").textContent =
                    productData.brandName;
                document.getElementById("quickViewProductColor").textContent =
                    productData.color;
                document.getElementById("quickViewProductPrice").textContent =
                    productData.basePrice.toFixed(2);
                document.getElementById("quickViewProductDescription").textContent =
                    productData.description;

                // Populate specifications
                document.getElementById("quickViewProductScreen").textContent =
                    productData.screen;
                document.getElementById("quickViewProductOS").textContent =
                    productData.os;
                document.getElementById("quickViewProductFrontCamera").textContent =
                    productData.frontCamera;
                document.getElementById("quickViewProductRearCamera").textContent =
                    productData.rearCamera;
                document.getElementById("quickViewProductCPU").textContent =
                    productData.cpu;
                document.getElementById("quickViewProductRAM").textContent =
                    productData.ram;
                document.getElementById("quickViewProductBattery").textContent =
                    productData.battery;
                document.getElementById("quickViewProductSIM").textContent =
                    productData.sim;
                document.getElementById("quickViewProductOther").textContent =
                    productData.other;

                // Update Add to Cart button and View Full Details link
                document
                    .querySelector(".add-to-cart-from-quickview")
                    .setAttribute("data-product-id", productId);
                document.getElementById(
                    "quickViewDetailsLink"
                ).href = `/Products/Details/${productId}`;

                // Clear and populate thumbnails (hardcoded for now)
                const thumbnailsContainer = document.getElementById(
                    "quickViewThumbnails"
                );
                thumbnailsContainer.innerHTML = "";
                productData.additionalImages.forEach((imgSrc) => {
                    const thumbImg = document.createElement("img");
                    thumbImg.src = imgSrc;
                    thumbImg.className = "img-thumbnail rounded-circle mx-1";
                    thumbImg.style.width = "60px";
                    thumbImg.style.height = "60px";
                    thumbImg.style.objectFit = "cover";
                    thumbImg.style.cursor = "pointer";
                    thumbImg.addEventListener("click", () => {
                        document.getElementById("quickViewProductImage").src = imgSrc;
                    });
                    thumbnailsContainer.appendChild(thumbImg);
                });

                productQuickViewModal.show();
            }
        });
    });

    // Handle add to cart from quick view modal
    document
        .querySelector(".add-to-cart-from-quickview")
        .addEventListener("click", function () {
            const productId = this.getAttribute("data-product-id");
            const productName = document.getElementById(
                "quickViewProductName"
            ).textContent;
            addToCart(productId, productName);
            productQuickViewModal.hide();
        });
});

// Utility functions
function addToCart(productId, productName) {
    // Show loading state
    const button = event.target;
    const originalText = button.innerHTML;
    button.innerHTML =
        '<span class="spinner-border spinner-border-sm me-2"></span>Adding...';
    button.disabled = true;

    // Simulate API call
    setTimeout(function () {
        // Update cart count
        const cartBadge = document.querySelector(".cart-badge");
        if (cartBadge) {
            const currentCount = parseInt(cartBadge.textContent) || 0;
            cartBadge.textContent = currentCount + 1;
        }

        // Show success message
        showToast(`${productName} added to cart!`, "success");

        // Reset button
        button.innerHTML = originalText;
        button.disabled = false;
    }, 1000);
}

function toggleWishlist(productId, button) {
    const icon = button.querySelector("i");

    if (icon.classList.contains("bi-heart")) {
        // Add to wishlist
        icon.classList.remove("bi-heart");
        icon.classList.add("bi-heart-fill");
        icon.style.color = "#dc3545"; // Red color for filled heart
        button.innerHTML =
            '<i class="bi bi-heart-fill"></i> Sản phẩm đã được thêm vào danh sách yêu thích';
        button.disabled = true; // Disable button after adding
        showToast("Sản phẩm đã được thêm vào danh sách yêu thích!", "success");
    } else {
        // This part handles removal, which isn't the primary request but good to keep consistent
        icon.classList.remove("bi-heart-fill");
        icon.classList.add("bi-heart");
        icon.style.color = ""; // Reset color
        button.innerHTML = '<i class="bi bi-heart"></i> Add to Wishlist';
        button.disabled = false;
        showToast("Removed from wishlist!", "info");
    }
}

function applyFilters() {
    const checkedFilters = document.querySelectorAll(".filter-checkbox:checked");
    const productCards = document.querySelectorAll(".product-card");

    productCards.forEach(function (card) {
        let shouldShow = true;

        checkedFilters.forEach(function (filter) {
            const filterType = filter.getAttribute("data-filter-type");
            const filterValue = filter.getAttribute("data-filter-value");
            const cardValue = card.getAttribute(`data-${filterType}`);

            if (cardValue && !cardValue.includes(filterValue)) {
                shouldShow = false;
            }
        });

        if (shouldShow) {
            card.style.display = "";
            card.classList.add("fade-in");
        } else {
            card.style.display = "none";
        }
    });
}

function submitReview(form) {
    const formData = new FormData(form);
    const submitBtn = form.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;

    submitBtn.innerHTML =
        '<span class="spinner-border spinner-border-sm me-2"></span>Submitting...';
    submitBtn.disabled = true;

    // Simulate API call
    setTimeout(function () {
        showToast("Review submitted successfully!", "success");
        form.reset();
        submitBtn.innerHTML = originalText;
        submitBtn.disabled = false;
    }, 1500);
}

function subscribeNewsletter(form) {
    const emailInput = form.querySelector('input[type="email"]');
    const email = emailInput.value;

    if (!email) {
        showToast("Please enter your email address.", "warning");
        return;
    }

    const apiUrl = "http://localhost:5241/api/Newsletter/subscribe"; // Assuming your API is running on this URL

    fetch(apiUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ email: email }),
    })
        .then((response) => {
            if (!response.ok) {
                return response.json().then((errorData) => {
                    throw new Error(errorData.message || "Subscription failed.");
                });
            }
            return response.json();
        })
        .then((data) => {
            showToast(data || "Subscription successful!", "success");
            emailInput.value = ""; // Clear the input field
        })
        .catch((error) => {
            console.error("Newsletter subscription error:", error);
            showToast(`Subscription failed: ${error.message}`, "danger");
        });
}

function showToast(message, type = "info") {
    const toastContainer = document.getElementById("toastContainer");
    if (!toastContainer) {
        const container = document.createElement("div");
        container.id = "toastContainer";
        container.className = "toast-container position-fixed top-0 end-0 p-3";
        container.style.zIndex = "9999";
        document.body.appendChild(container);
    }

    const toastHtml = `
        <div class="toast align-items-center text-white bg-${type} border-0" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        </div>
    `;

    toastContainer.insertAdjacentHTML("beforeend", toastHtml);
    const toastElement = toastContainer.lastElementChild;
    const toast = new bootstrap.Toast(toastElement);
    toast.show();

    // Remove toast element after it's hidden
    toastElement.addEventListener("hidden.bs.toast", function () {
        toastElement.remove();
    });
}

// Cart functions
function updateCartQuantity(itemId, quantity) {
    // Update cart item quantity
    const quantityElement = document.querySelector(
        `[data-cart-item="${itemId}"] .quantity-input`
    );
    if (quantityElement) {
        quantityElement.value = quantity;
        updateCartTotal();
    }
}

function removeCartItem(itemId) {
    const cartItem = document.querySelector(`[data-cart-item="${itemId}"]`);
    if (cartItem) {
        cartItem.remove();
        updateCartTotal();
        showToast("Item removed from cart!", "info");
    }
}

function updateCartTotal() {
    const cartItems = document.querySelectorAll(".cart-item");
    let total = 0;

    cartItems.forEach(function (item) {
        const price = parseFloat(item.getAttribute("data-price"));
        const quantity = parseInt(item.querySelector(".quantity-input").value);
        total += price * quantity;
    });

    const totalElement = document.getElementById("cartTotal");
    if (totalElement) {
        totalElement.textContent = "$" + total.toFixed(2);
    }
}

function getHardcodedProductDetails(productId) {
    // This function simulates fetching product details. In a real application,
    // you would make an AJAX call to your API here.
    switch (parseInt(productId)) {
        case 1:
            return {
                phoneId: 1,
                phoneName: "iPhone 15 Pro",
                brandName: "Apple",
                imageUrl: "/images/apple_iphone_15_pro.jpg",
                basePrice: 999.0,
                color: "Black",
                description:
                    "The latest and most powerful iPhone with a stunning display and advanced camera system.",
                screen: "6.1-inch Super Retina XDR",
                os: "iOS 17",
                frontCamera: "12MP TrueDepth",
                rearCamera: "48MP Main, 12MP Ultra Wide, 12MP Telephoto",
                cpu: "A17 Bionic Chip",
                ram: "8GB",
                battery: "All-day battery life",
                sim: "Dual SIM (nano-SIM and eSIM)",
                other: "Titanium design, USB-C, Action Button",
                additionalImages: [
                    "/images/apple_iphone_15_pro.jpg",
                    "/images/apple_iphone_15_pro_2.jpg", // Placeholder
                    "/images/apple_iphone_15_pro_3.jpg", // Placeholder
                ],
            };
        case 2:
            return {
                phoneId: 2,
                phoneName: "Samsung Galaxy S24 Ultra",
                brandName: "Samsung",
                imageUrl: "/images/samsung_galaxy_s24_ultra.jpg",
                basePrice: 1199.0,
                color: "Titanium Gray",
                description:
                    "Experience the pinnacle of mobile innovation with the Galaxy S24 Ultra, featuring an S Pen and groundbreaking AI.",
                screen: "6.8-inch Dynamic AMOLED 2X",
                os: "Android 14",
                frontCamera: "12MP",
                rearCamera:
                    "200MP Wide, 50MP Periscope Telephoto, 10MP Telephoto, 12MP Ultra Wide",
                cpu: "Snapdragon 8 Gen 3 for Galaxy",
                ram: "12GB",
                battery: "5000 mAh",
                sim: "Dual SIM (nano-SIM and eSIM)",
                other: "S Pen, Galaxy AI, Titanium Frame",
                additionalImages: [
                    "/images/samsung_galaxy_s24_ultra.jpg",
                    "/images/samsung_galaxy_s24_ultra_2.jpg", // Placeholder
                    "/images/samsung_galaxy_s24_ultra_3.jpg", // Placeholder
                ],
            };
        case 3:
            return {
                phoneId: 3,
                phoneName: "Google Pixel 8 Pro",
                brandName: "Google",
                imageUrl: "/images/google_pixel_8_pro.jpg",
                basePrice: 799.0,
                color: "Obsidian",
                description:
                    "Google's most advanced Pixel yet, powered by Google AI for incredible photos and helpful features.",
                screen: "6.7-inch Super Actua display",
                os: "Android 14",
                frontCamera: "10.5MP",
                rearCamera: "50MP Wide, 48MP Ultrawide, 48MP Telephoto",
                cpu: "Google Tensor G3",
                ram: "12GB",
                battery: "Beyond 24-hour battery life",
                sim: "Dual SIM (nano-SIM and eSIM)",
                other: "Thermometer, Magic Editor, Best Take",
                additionalImages: [
                    "/images/google_pixel_8_pro.jpg",
                    "/images/google_pixel_8_pro_2.jpg", // Placeholder
                    "/images/google_pixel_8_pro_3.jpg", // Placeholder
                ],
            };
        // Add more hardcoded products as needed for demonstration
        default:
            return null;
    }
}
