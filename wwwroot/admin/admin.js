/* Admin shared code: sample data store, sidebar/topbar, small helpers.
   TODO (backend): replace db.all() / db.save() with fetch() calls to your API. */

const SEED = {
  cats: [
    { id: 1, name: "السياسة", icon: "bank" },
    { id: 2, name: "التكنولوجيا", icon: "cpu" },
    { id: 3, name: "الرياضة", icon: "trophy" },
    { id: 4, name: "الاقتصاد", icon: "graph-up-arrow" },
    { id: 5, name: "الترفيه", icon: "film" },
    { id: 6, name: "الصحة", icon: "heart-pulse" },
  ],
  news: [
    { id: 1, title: "البرلمان يناقش مشروع قانون الاستثمار الجديد", cat: "السياسة", author: "سارة عبد الله", status: "published", date: "2026-10-05", image: "Images/news-1.jpg", desc: "جلسة مطولة لمناقشة حوافز المستثمرين وتبسيط الإجراءات.", content: "ناقش البرلمان في جلسته اليوم مشروع القانون الجديد وسط تباين في الآراء بين النواب." },
    { id: 2, title: "تطبيق عربي جديد يترجم المحادثات فوريًا", cat: "التكنولوجيا", author: "ليلى حسين", status: "published", date: "2026-10-05", image: "Images/news-2.jpg", desc: "يدعم التطبيق أكثر من 40 لغة ويعمل دون إنترنت.", content: "أطلقت شركة ناشئة تطبيقًا يعتمد على الذكاء الاصطناعي لترجمة المحادثات الصوتية فورًا." },
    { id: 3, title: "المنتخب يستعد لتصفيات كأس الأمم بقائمة موسعة", cat: "الرياضة", author: "يوسف كمال", status: "review", date: "2026-10-04", image: "Images/news-3.jpg", desc: "أعلن الجهاز الفني قائمة تضم وجوهًا جديدة.", content: "ضمت القائمة 28 لاعبًا يبدأون معسكرًا مغلقًا الأسبوع المقبل." },
    { id: 4, title: "ارتفاع صادرات القطاع الصناعي بنسبة 12%", cat: "الاقتصاد", author: "سارة عبد الله", status: "published", date: "2026-10-04", image: "Images/news-4.jpg", desc: "نمو ملحوظ في صادرات الأدوية والمنسوجات.", content: "أظهرت بيانات رسمية نموًا في الصادرات الصناعية خلال الربع الثالث من العام." },
    { id: 5, title: "انطلاق مهرجان القاهرة السينمائي بعروض أولى", cat: "الترفيه", author: "عمر فاروق", status: "draft", date: "2026-10-03", image: "Images/news-5.jpg", desc: "يشارك أكثر من 80 فيلمًا من 30 دولة.", content: "افتتحت فعاليات المهرجان بحضور نخبة من صناع السينما." },
    { id: 6, title: "دراسة: المشي 30 دقيقة يوميًا يقلل مخاطر القلب", cat: "الصحة", author: "ليلى حسين", status: "published", date: "2026-10-03", image: "Images/news-6.jpg", desc: "توصي الدراسة بنشاط معتدل خمسة أيام أسبوعيًا.", content: "تابعت الدراسة آلاف المشاركين على مدى عشر سنوات." },
  ],
  msgs: [
    { id: 1, name: "هاني رمضان", email: "hany@example.com", subject: "استفسار عن الإعلانات", message: "أرغب في معرفة أسعار الإعلانات على الموقع.", date: "2026-10-06" },
    { id: 2, name: "دينا فتحي", email: "dina@example.com", subject: "تصحيح خبر", message: "يوجد خطأ في اسم الشركة في خبر الأمس.", date: "2026-10-05" },
    { id: 3, name: "كريم سمير", email: "karim@example.com", subject: "فرصة تدريب", message: "هل يوجد برنامج تدريب للطلاب في قسم التحرير؟", date: "2026-10-04" },
    { id: 4, name: "هبة محمود", email: "heba@example.com", subject: "اقتراح", message: "أقترح إضافة نشرة بريدية يومية.", date: "2026-10-02" },
  ],
  comments: [
    { id: 1, name: "محمد علي", rating: 5, comment: "تغطية ممتازة وسريعة، والتصميم مريح للقراءة.", date: "2026-10-03", avatar: "Images/avatar-1.jpg" },
    { id: 2, name: "منى حسن", rating: 4, comment: "أحب قسم الاقتصاد، أتمنى زيادة التقارير المصورة.", date: "2026-10-01", avatar: "Images/avatar-2.jpg" },
    { id: 3, name: "خالد إبراهيم", rating: 5, comment: "موقع موثوق ومحتوى متنوع. شكرًا لكم.", date: "2026-09-28", avatar: "Images/avatar-3.jpg" },
    { id: 4, name: "نور السيد", rating: 4, comment: "الأخبار الرياضية دقيقة، وأرجو إضافة نشرة صوتية.", date: "2026-09-25", avatar: "Images/avatar-4.jpg" },
  ],
};

/* In-memory cache + localStorage, so changes survive a page reload. */
const mem = {};
const db = {
  all(key) {
    if (!mem[key]) {
      try { mem[key] = JSON.parse(localStorage.getItem("rased_" + key)); } catch (e) { /* ignore */ }
      if (!mem[key]) mem[key] = JSON.parse(JSON.stringify(SEED[key]));
    }
    return mem[key];
  },
  save(key, list) {
    mem[key] = list;
    try { localStorage.setItem("rased_" + key, JSON.stringify(list)); } catch (e) { /* ignore */ }
  },
  nextId: (list) => list.reduce((m, x) => Math.max(m, x.id), 0) + 1,
};

const esc = (s) => String(s).replace(/[&<>"']/g, (c) => ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" }[c]));
const stars = (n) => `<span class="stars text-warning">${[1, 2, 3, 4, 5].map((i) => `<i class="bi bi-star${i <= n ? "-fill" : ""}"></i>`).join("")}</span>`;
const actions = (editFn, delFn, id) => `
  <a href="#" class="icon-btn" title="تعديل" onclick="${editFn}(${id});return false"><i class="bi bi-pencil"></i></a>
  <a href="#" class="icon-btn danger" title="حذف" onclick="${delFn}(${id});return false"><i class="bi bi-trash"></i></a>`;

/* Sidebar + topbar (same on every admin page) */
const NAV = [
  ["dashboard.html", "speedometer2", "لوحة التحكم"],
  ["admin-news.html", "newspaper", "الأخبار"],
  ["admin-categories.html", "grid", "التصنيفات"],
  ["admin-comments.html", "chat-dots", "التعليقات"],
  ["admin-contact.html", "envelope", "رسائل التواصل"],
];
const here = location.pathname.split("/").pop();

document.getElementById("admin-side").innerHTML = `
  <div class="offcanvas-lg offcanvas-start" id="side" tabindex="-1">
    <div class="offcanvas-header d-lg-none">
      <button class="btn-close btn-close-white" data-bs-dismiss="offcanvas" data-bs-target="#side" aria-label="إغلاق"></button>
    </div>
    <div class="offcanvas-body d-block p-0">
      <a class="side-brand brand" href="index.html"><i class="bi bi-broadcast-pin"></i> الراصد</a>
      ${NAV.map(([href, icon, label]) => `
        <a class="side-link ${href === here ? "active" : ""}" href="${href}"><i class="bi bi-${icon}"></i> ${label}</a>`).join("")}
      <a class="side-link" href="index.html"><i class="bi bi-box-arrow-up-left"></i> عرض الموقع</a>
    </div>
  </div>`;

document.getElementById("admin-top").innerHTML = `
  <button class="btn btn-outline-secondary d-lg-none" data-bs-toggle="offcanvas" data-bs-target="#side" aria-label="القائمة"><i class="bi bi-list"></i></button>
  <strong class="flex-grow-1">${esc(document.title.split("|")[0])}</strong>
  <img src="Images/team-1.jpg" alt="" width="38" height="38" class="rounded-circle object-fit-cover">
  <span class="d-none d-md-inline fw-bold">أحمد المنصور</span>`;
