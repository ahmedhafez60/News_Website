"""Download the real placeholder photos into ./Images (run once, needs internet).
Usage:  python download_images.py
Files keep the same names, so the HTML needs no changes. Replace any of them with your own photos later."""
import os, urllib.request
IMAGES = {
    "hero-summit.jpg": "https://picsum.photos/seed/summit/1600/800",
    "hero-chip.jpg": "https://picsum.photos/seed/chip/1600/800",
    "hero-stadium.jpg": "https://picsum.photos/seed/stadium/1600/800",
    "hero-market.jpg": "https://picsum.photos/seed/market/1600/800",
    "news-1.jpg": "https://picsum.photos/seed/a1/700/440",
    "news-2.jpg": "https://picsum.photos/seed/a2/700/440",
    "news-3.jpg": "https://picsum.photos/seed/a3/700/440",
    "news-4.jpg": "https://picsum.photos/seed/a4/700/440",
    "news-5.jpg": "https://picsum.photos/seed/a5/700/440",
    "news-6.jpg": "https://picsum.photos/seed/a6/700/440",
    "article-featured.jpg": "https://picsum.photos/seed/market/1400/700",
    "article-trading.jpg": "https://picsum.photos/seed/trading/900/500",
    "article-bank.jpg": "https://picsum.photos/seed/bank/900/500",
    "related-1.jpg": "https://picsum.photos/seed/r1/700/440",
    "related-2.jpg": "https://picsum.photos/seed/r2/700/440",
    "related-3.jpg": "https://picsum.photos/seed/r3/700/440",
    "related-4.jpg": "https://picsum.photos/seed/r4/700/440",
    "avatar-1.jpg": "https://i.pravatar.cc/120?img=12",
    "avatar-2.jpg": "https://i.pravatar.cc/120?img=47",
    "avatar-3.jpg": "https://i.pravatar.cc/120?img=33",
    "avatar-4.jpg": "https://i.pravatar.cc/120?img=25",
    "team-1.jpg": "https://i.pravatar.cc/300?img=11",
    "team-2.jpg": "https://i.pravatar.cc/300?img=45",
    "team-3.jpg": "https://i.pravatar.cc/300?img=15",
    "team-4.jpg": "https://i.pravatar.cc/300?img=32",
    "team-5.jpg": "https://i.pravatar.cc/300?img=60",
    "team-6.jpg": "https://i.pravatar.cc/300?img=26",
    "banner.jpg": "https://picsum.photos/seed/newsroom/1600/500",
}
os.makedirs("Images", exist_ok=True)
for name, url in IMAGES.items():
    try:
        req = urllib.request.Request(url, headers={"User-Agent": "Mozilla/5.0"})
        with urllib.request.urlopen(req, timeout=30) as r, open(os.path.join("Images", name), "wb") as f:
            f.write(r.read())
        print("OK  ", name)
    except Exception as e:
        print("FAIL", name, e)
