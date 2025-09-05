// Admin Dashboard JavaScript

document.addEventListener("DOMContentLoaded", function () {
  // Sidebar toggle
  const sidebarToggle = document.getElementById("sidebarToggle");
  const wrapper = document.getElementById("wrapper");

  if (sidebarToggle) {
    sidebarToggle.addEventListener("click", function (e) {
      e.preventDefault();
      wrapper.classList.toggle("toggled");
    });
  }

  // Auto-hide alerts after 5 seconds
  const alerts = document.querySelectorAll(".alert");
  alerts.forEach(function (alert) {
    setTimeout(function () {
      const bsAlert = new bootstrap.Alert(alert);
      bsAlert.close();
    }, 5000);
  });

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

  // Confirm delete modals
  const deleteButtons = document.querySelectorAll("[data-confirm-delete]");
  deleteButtons.forEach(function (button) {
    button.addEventListener("click", function (e) {
      const message =
        this.getAttribute("data-confirm-delete") ||
        "Are you sure you want to delete this item?";
      if (!confirm(message)) {
        e.preventDefault();
        return false;
      }
    });
  });

  // Search functionality
  const searchInput = document.getElementById("searchInput");
  if (searchInput) {
    searchInput.addEventListener("input", function () {
      const searchTerm = this.value.toLowerCase();
      const tableRows = document.querySelectorAll("tbody tr");

      tableRows.forEach(function (row) {
        const text = row.textContent.toLowerCase();
        if (text.includes(searchTerm)) {
          row.style.display = "";
        } else {
          row.style.display = "none";
        }
      });
    });
  }

  // Bulk actions
  const selectAllCheckbox = document.getElementById("selectAll");
  const itemCheckboxes = document.querySelectorAll(".item-checkbox");
  const bulkActionBtn = document.getElementById("bulkActionBtn");

  if (selectAllCheckbox) {
    selectAllCheckbox.addEventListener("change", function () {
      itemCheckboxes.forEach(function (checkbox) {
        checkbox.checked = selectAllCheckbox.checked;
      });
      updateBulkActionButton();
    });
  }

  itemCheckboxes.forEach(function (checkbox) {
    checkbox.addEventListener("change", function () {
      updateBulkActionButton();

      // Update select all checkbox
      const checkedCount = document.querySelectorAll(
        ".item-checkbox:checked"
      ).length;
      const totalCount = itemCheckboxes.length;

      if (selectAllCheckbox) {
        selectAllCheckbox.checked = checkedCount === totalCount;
        selectAllCheckbox.indeterminate =
          checkedCount > 0 && checkedCount < totalCount;
      }
    });
  });

  function updateBulkActionButton() {
    const checkedCount = document.querySelectorAll(
      ".item-checkbox:checked"
    ).length;
    if (bulkActionBtn) {
      bulkActionBtn.disabled = checkedCount === 0;
      bulkActionBtn.textContent = `Apply to ${checkedCount} item(s)`;
    }
  }

  // Form validation
  const forms = document.querySelectorAll(".needs-validation");
  forms.forEach(function (form) {
    form.addEventListener("submit", function (event) {
      if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
      }
      form.classList.add("was-validated");
    });
  });

  // File upload preview
  const fileInputs = document.querySelectorAll(".custom-file-input");
  fileInputs.forEach(function (input) {
    input.addEventListener("change", function () {
      const fileName = this.files[0]?.name || "Choose file...";
      const label = this.nextElementSibling;
      if (label) {
        label.textContent = fileName;
      }
    });
  });

  // Dynamic form fields
  const addFieldButtons = document.querySelectorAll(".add-field-btn");
  addFieldButtons.forEach(function (button) {
    button.addEventListener("click", function () {
      const container = document.querySelector(
        this.getAttribute("data-target")
      );
      const template = document.querySelector(
        this.getAttribute("data-template")
      );

      if (container && template) {
        const clone = template.content.cloneNode(true);
        container.appendChild(clone);
      }
    });
  });

  // Remove field buttons
  document.addEventListener("click", function (e) {
    if (e.target.classList.contains("remove-field-btn")) {
      e.target.closest(".field-group").remove();
    }
  });

  // Tab navigation with URL hash
  const hash = window.location.hash;
  if (hash) {
    const tab = document.querySelector(`[data-bs-target="${hash}"]`);
    if (tab) {
      const tabInstance = new bootstrap.Tab(tab);
      tabInstance.show();
    }
  }

  // Update URL hash when tabs are clicked
  const tabButtons = document.querySelectorAll('[data-bs-toggle="tab"]');
  tabButtons.forEach(function (button) {
    button.addEventListener("shown.bs.tab", function (e) {
      const target = e.target.getAttribute("data-bs-target");
      if (target) {
        window.location.hash = target;
      }
    });
  });
});

// Utility functions
function showLoading(element) {
  if (element) {
    element.disabled = true;
    element.innerHTML =
      '<span class="spinner-border spinner-border-sm me-2"></span>Loading...';
  }
}

function hideLoading(element, originalText) {
  if (element) {
    element.disabled = false;
    element.innerHTML = originalText;
  }
}

function showToast(message, type = "info") {
  const toastContainer = document.getElementById("toastContainer");
  if (!toastContainer) {
    // Create toast container if it doesn't exist
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
