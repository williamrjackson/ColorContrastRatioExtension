# Unity Color Contrast Ratio Extension Method
Color extension method to modify a color to meet low vision accessibility contrast ratio guidlines    

https://www.w3.org/WAI/WCAG21/Understanding/contrast-minimum.html    
https://www.w3.org/TR/WCAG20-TECHS/G17.html

Usage:    
`text.color = text.color.EnsureContrastRatio(backgroundImage.color, targetRatio);`
