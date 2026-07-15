#!/usr/bin/env python3
"""Print pages of exam_guide.pdf that mention a topic. Usage: read_guide.py <topic>"""
import sys
from pathlib import Path
from pypdf import PdfReader  # pip install pypdf

GUIDE = Path(__file__).resolve().parent.parent / "exam_guide.pdf"

topic = sys.argv[1].lower() if len(sys.argv) > 1 else ""
reader = PdfReader(GUIDE)

for n, page in enumerate(reader.pages, 1):
    text = page.extract_text() or ""
    if topic in text.lower():
        print(f"\n===== page {n} =====")
        print(text.strip())
