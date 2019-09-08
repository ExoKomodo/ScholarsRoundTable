SELECT x.id as book_id, x.name as name, y.commentary_id as commentary_id
FROM (
    SELECT id, name
    FROM Books
) AS x
INNER JOIN (
    SELECT commentary_id, book_id
    FROM CommentaryBook
    WHERE commentary_id = {commentary_id}
) AS y ON x.id = y.book_id;