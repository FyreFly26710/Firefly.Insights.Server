using System;

namespace Server.Ai.Api.Infrastructure.AiClients;

public class RulePrompts
{
    public static string StructureRules = """
    Generate a JSON response in this EXACT structure:
    {
    "Articles": [
        {
        "SortNumber": 1,
        "Title": "Rapid HTML Enhancement & Best Practices",
        "Description": Your concise description written in plain text (DO NOT USE MARKDOWN)",
        "SkillLevelTag": "Advanced",
        "FocusAreaTag": "Performance Optimization",
        "ArticleStyleTag": "Best-practices",
        "TechStackTag": "HTML",
        "ToneTag": "Technical"
        }
    ],
    "AiMessage": "Your message here."
    }   
    """;

    public static string TagRules = """
    Tags list:
    1. **SkillLevelTag**: Beginner / Advanced / Expert / General  
    2. **FocusAreaTag**: (choose one relevant to the article, e.g. "API Design," "Performance Optimization," "Testing")  
    3. **TechStackTag**: (e.g. ORM," "JavaScript," "Kubernetes"; only when applicable, if not applicable, leave blank)  
    4. **ArticleStyleTag**: Overview / Deep-dive / Best-practices / Listicle / Q&A / Comparison  
    5. **ToneTag**: Conversational / Academic / Technical / Code-heavy

    Tag Interpretation Rules:
    - **Skill Level**  
    • Beginner → assume no prior knowledge.
    • Advanced → assume some experience; use domain terms with brief clarifications.  
    • Expert → presume deep background; dive into edge cases, optimizations, trade-offs.  
    • General → not applicable to above or mix of levels; focus on broad understanding. Placeholder for articles that are not applicable to above styles.

    - **Focus Area**  
    → Choose examples, case studies, or sections that best match this subdomain.

    - **Tech Stack/Language**  
    → Write all code samples, configuration snippets, and idiomatic examples in this stack.  
    → Follow its best practices and naming conventions.
    → Optional, if not applicable, leave blank.

    - **Article Style**  
    • Overview → broad survey with high-level descriptions.  
    • Deep-dive → long-form sections, detailed explanations, diagrams or pseudocode.  
    • Best-practices → "Do's and Don'ts," common pitfalls, recommended patterns.  
    • Listicle → numbered or bulleted list of key points.  
    • Q&A → simulate an interview or FAQ format.  
    • Comparison → side-by-side pros/cons, feature matrix or benchmark notes.

    - **Tone**  
    • Academic → formal style, careful choice of words.  
    • Technical → mix of prose and inline code comments; clear instructions.  
    • Code-heavy → dense with code examples, minimal prose.
    • Conversational → informal, casual, engaging tone. Placeholder for articles that are not applicable to above styles.

    """;

    public static string AllowedMarkdownElements = """
    Only use the following markdown elements in your responses:
    User them appropriately to enhance readability and structure.

    Plain Text

    # H1
    ## H2
    ### H3

    **bold text**

    *italicized text*

    > blockquote

    ### Ordered List

    1. First item
    2. Second item
    3. Third item

    ### Unordered List

    - First item
    - Second item
    - Third item

    `code`

    ### Horizontal Rule

    ---

    ### Link

    [Markdown Guide](https://www.markdownguide.org)

    ### Table

    | Syntax | Description |
    | ----------- | ----------- |
    | Header | Title |
    | Paragraph | Text |

    ### Fenced Code Block

    ```
    {
    "firstName": "John",
    "lastName": "Smith",
    "age": 25
    }
    ```

    ``` Csharp
    static void BubbleSort(int[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }
    ```

    ### Footnote

    Here's a sentence with a footnote. [^1]

    [^1]: This is the footnote.

    ### Strikethrough

    ~~The world is flat.~~

    ### Task List

    - [x] Write the press release
    - [ ] Update the website
    - [ ] Contact the media

    ### Subscript

    H~2~O

    ### Superscript

    X^2^

    """;
}
