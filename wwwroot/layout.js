/* Shared layout: injects the same navbar and footer on every page,
   and enables Bootstrap validation on forms marked .needs-validation. */

const PAGES = [
  { href: "index.html", label: "الرئيسية" },
  { href: "news.html", label: "الأخبار" },
  { href: "contact.html", label: "اتصل بنا" },
  { href: "team.html", label: "فريق العمل" },
];

const CATEGORIES = [
  { label: "السياسة", icon: "bank" },
  { label: "التكنولوجيا", icon: "cpu" },
  { label: "الرياضة", icon: "trophy" },
  { label: "الاقتصاد", icon: "graph-up-arrow" },
  { label: "الترفيه", icon: "film" },
  { label: "الصحة", icon: "heart-pulse" },
];

const SOCIAL = ["facebook", "twitter-x", "instagram", "youtube"];
const current = location.pathname.split("/").pop() || "index.html";

const navLink = ({ href, label }) => `
  <li class="nav-item">
    <a class="nav-link ${href === current ? "active" : ""}" href="${href}">${label}</a>
  </li>`;

const socialLinks = () =>
  SOCIAL.map((s) => `<a href="#" aria-label="${s}"><i class="bi bi-${s}"></i></a>`).join("");

function renderNavbar() {
  const categoryItems = CATEGORIES.map(
    (c) => `
      <li>
        <a class="dropdown-item" href="index.html#categories">
          <i class="bi bi-${c.icon} me-2"></i>${c.label}
        </a>
      </li>`
  ).join("");

  document.getElementById("site-nav").innerHTML = `
    <nav class="navbar navbar-expand-lg navbar-dark sticky-top">
      <div class="container">
        <a class="brand" href="index.html"><i class="bi bi-broadcast-pin"></i> الراصد</a>
        <button class="navbar-toggler" data-bs-toggle="collapse" data-bs-target="#menu" aria-label="القائمة">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="menu">
          <ul class="navbar-nav ms-auto">
            ${navLink(PAGES[0])}
            ${navLink(PAGES[1])}
            <li class="nav-item dropdown">
              <a class="nav-link dropdown-toggle" href="index.html#categories" data-bs-toggle="dropdown">التصنيفات</a>
              <ul class="dropdown-menu">${categoryItems}</ul>
            </li>
            ${navLink(PAGES[2])}
            ${navLink(PAGES[3])}
          </ul>
        </div>
      </div>
    </nav>`;
}

function renderFooter() {
  const quickLinks = PAGES.map((p) => `<li><a href="${p.href}">${p.label}</a></li>`).join("");
  const categoryLinks = CATEGORIES.map(
    (c) => `<li><a href="index.html#categories">${c.label}</a></li>`
  ).join("");

  document.getElementById("site-footer").innerHTML = `
    <footer class="pt-5">
      <div class="container">
        <div class="row g-4">
          <div class="col-lg-4">
            <div class="brand text-white fs-3"><i class="bi bi-broadcast-pin text-danger"></i> الراصد</div>
            <p class="mt-2">موقع إخباري عربي يقدّم تغطية دقيقة وموثوقة للأحداث المحلية والعالمية على مدار الساعة.</p>
            <div class="social">${socialLinks()}</div>
          </div>
          <div class="col-6 col-lg-4">
            <h6>روابط سريعة</h6>
            <ul class="list-unstyled">${quickLinks}</ul>
          </div>
          <div class="col-6 col-lg-4">
            <h6>التصنيفات</h6>
            <ul class="list-unstyled">${categoryLinks}</ul>
          </div>
        </div>
        <hr class="border-secondary mt-4">
        <p class="text-center small pb-3 mb-0">© 2026 الراصد. جميع الحقوق محفوظة.</p>
      </div>
    </footer>`;
}

function initFormValidation() {
  document.querySelectorAll(".needs-validation").forEach((form) => {
    form.addEventListener("submit", (event) => {
      event.preventDefault();
      if (form.checkValidity()) {
        // TODO (backend): send the form data to your API here.
        form.reset();
        form.classList.remove("was-validated");
        alert("تم الإرسال بنجاح");
      } else {
        form.classList.add("was-validated");
      }
    });
  });
}

// renderNavbar();
// renderFooter();
// initFormValidation();
