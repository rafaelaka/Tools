# Tools

## TemplateCSVMerger

### Usage: TemplateCSVMerger &lt;template_file&gt; &lt;csv_file&gt; [&lt;column_name_with_output_filename&gt;]

This application merges a template file with placeholders with the rows of a CSV file and generates one file for each row.
The placeholders have the format '«'column name'»'.
The first row of the CSV file contains the column names to be replaced by the column value in each row.
As he replacement is case and culture sensitive, confirm that both files have the same encoding.
Rows starting with # are not considered.
CSV delimiter is ';'

