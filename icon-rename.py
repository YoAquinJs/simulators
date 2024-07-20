"""Rename dowloaded icons to icon{index} for portability"""

import os

COLOR = "#087990"
PATH = "./icons/"
FILE_END = "</svg>"

for i, filename in enumerate(os.listdir(PATH)):
    if filename.startswith("icon"):
        continue

    new_name = f"icon{i}.svg"
    os.rename(f"{PATH}{filename}", f"{PATH}{new_name}")
