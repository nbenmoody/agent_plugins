# /// script
# requires-python = ">=3.10"
# dependencies = [
#     "mcp[cli]>=1.0",
#     "pypdf>=4.0",
# ]
# ///
from pathlib import Path
import html
from mcp.server.fastmcp import FastMCP

mcp = FastMCP("training-factory")

@mcp.tool()
def ping(name: str) -> str:
    """Return a friendly greeting. Use to check the server is alive."""
    return f"training-factory is running, {name}."

# templates/ sits next to this file, so resolve relative to __file__
TEMPLATE = (Path(__file__).parent.parent / "templates" / "training.html")

def _render_section(i: int, sec: dict) -> str:
    """Turn one {title, body} dict into a card, matching the house format."""
    title = html.escape(sec["title"])
    body  = sec["body"]  # trusted HTML the caller supplies
    return f'''
  <section class="card" data-sec="{i}">
    <div class="sec-head"><span class="sec-num">Section {i}</span></div>
    <h2>{title}</h2>
    {body}
    <div class="mark"><label><input type="checkbox"> Mark this section complete</label></div>
  </section>'''

@mcp.tool()
def create_training(title: str, sections: list[dict], out_dir: str = ".") -> str:
    """Generate an HTML training in the standard format.

    Use when the user asks to create a training, lesson, or study guide.
    Each item in `sections` is a dict: {"title": str, "body": "<p>…</p> HTML"}.
    Keep it to 3-5 sections so the result stays a 5-10 minute read.
    Returns the path to the written .html file."""
    if not (3 <= len(sections) <= 5):
        raise ValueError("Provide between 3 and 5 sections.")

    cards = "".join(_render_section(i, s) for i, s in enumerate(sections, 1))
    doc = (TEMPLATE.read_text()
           .replace("__TITLE__", html.escape(title))
           .replace("__SECTIONS__", cards))

    slug = title.lower().replace(" ", "-")
    path = Path(out_dir) / f"training-{slug}.html"
    path.write_text(doc)
    return f"Wrote training to {path.resolve()}"


if __name__ == "__main__":
    mcp.run()
