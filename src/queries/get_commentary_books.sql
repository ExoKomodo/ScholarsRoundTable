SELECT x.book_id, y.book_name, y.commentary_id, y.commentary_name
FROM (
    SELECT book_id, commentary_id
    FROM CommentaryBook
) AS x
INNER JOIN (
    SELECT Books.id AS book_id, Books.name AS book_name, Commentaries.id AS commentary_id, Commentaries.name AS commentary_name
    FROM Books, Commentaries
) AS y ON x.book_id = y.book_id and x.commentary_id = y.commentary_id
ORDER BY y.commentary_name;