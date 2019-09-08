SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS `Authorities`;
DROP TABLE IF EXISTS `Books`;
DROP TABLE IF EXISTS `Commentaries`;
DROP TABLE IF EXISTS `CommentaryBook`;
DROP TABLE IF EXISTS `Rankings`;
DROP TABLE IF EXISTS `Seminaries`;

SET FOREIGN_KEY_CHECKS = 1;

CREATE TABLE IF NOT EXISTS `Users` (
	`id` VARCHAR(255) NOT NULL,
	`is_admin` BOOLEAN NOT NULL,
	`profile_pic` TEXT NOT NULL,
  	`email` VARCHAR(255) UNIQUE NOT NULL,
  	`name` TEXT NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Authorities` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(255) NOT NULL,
	`seminary_id` INT NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Seminaries` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Rankings` (
	`authority_id` INT NOT NULL,
	`commentary_id` INT NOT NULL,
    `book_id` INT NOT NULL,
	`ranking` FLOAT NOT NULL,
	PRIMARY KEY (`authority_id`, `commentary_id`, `book_id`)
);

CREATE TABLE IF NOT EXISTS `Commentaries` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(255) NOT NULL,
	`isbn` VARCHAR(13) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Books` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(255) NOT NULL UNIQUE,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `CommentaryBook` (
	`book_id` INT NOT NULL,
	`commentary_id` INT NOT NULL,
	PRIMARY KEY (`book_id`, `commentary_id`)
);

ALTER TABLE `Authorities` ADD CONSTRAINT `Authorities_fk0` FOREIGN KEY (`seminary_id`) REFERENCES `Seminaries`(`id`);

ALTER TABLE `Rankings` ADD CONSTRAINT `Rankings_fk0` FOREIGN KEY (`authority_id`) REFERENCES `Authorities`(`id`);

ALTER TABLE `Rankings` ADD CONSTRAINT `Rankings_fk1` FOREIGN KEY (`book_id`, `commentary_id`) REFERENCES `CommentaryBook`(`book_id`, `commentary_id`);

ALTER TABLE `CommentaryBook` ADD CONSTRAINT `CommentaryBook_fk0` FOREIGN KEY (`book_id`) REFERENCES `Books`(`id`);

ALTER TABLE `CommentaryBook` ADD CONSTRAINT `CommentaryBook_fk1` FOREIGN KEY (`commentary_id`) REFERENCES `Commentaries`(`id`);

INSERT INTO `Books` (`name`) VALUES ('Genesis');
INSERT INTO `Books` (`name`) VALUES ('Exodus');
INSERT INTO `Books` (`name`) VALUES ('Leviticus');
INSERT INTO `Books` (`name`) VALUES ('Numbers');
INSERT INTO `Books` (`name`) VALUES ('Deuteronomy');
INSERT INTO `Books` (`name`) VALUES ('Joshua');
INSERT INTO `Books` (`name`) VALUES ('Judges');
INSERT INTO `Books` (`name`) VALUES ('Ruth');
INSERT INTO `Books` (`name`) VALUES ('1 Samuel');
INSERT INTO `Books` (`name`) VALUES ('2 Samuel');
INSERT INTO `Books` (`name`) VALUES ('1 Kings');
INSERT INTO `Books` (`name`) VALUES ('2 Kings');
INSERT INTO `Books` (`name`) VALUES ('1 Chronicles');
INSERT INTO `Books` (`name`) VALUES ('2 Chronicles');
INSERT INTO `Books` (`name`) VALUES ('Ezra');
INSERT INTO `Books` (`name`) VALUES ('Nehemiah');
INSERT INTO `Books` (`name`) VALUES ('Esther');
INSERT INTO `Books` (`name`) VALUES ('Job');
INSERT INTO `Books` (`name`) VALUES ('Psalm');
INSERT INTO `Books` (`name`) VALUES ('Proverbs');
INSERT INTO `Books` (`name`) VALUES ('Ecclesiastes');
INSERT INTO `Books` (`name`) VALUES ('Song of Songs');
INSERT INTO `Books` (`name`) VALUES ('Isaiah');
INSERT INTO `Books` (`name`) VALUES ('Jeremiah');
INSERT INTO `Books` (`name`) VALUES ('Lamentations');
INSERT INTO `Books` (`name`) VALUES ('Ezekiel');
INSERT INTO `Books` (`name`) VALUES ('Daniel');
INSERT INTO `Books` (`name`) VALUES ('Hosea');
INSERT INTO `Books` (`name`) VALUES ('Joel');
INSERT INTO `Books` (`name`) VALUES ('Amos');
INSERT INTO `Books` (`name`) VALUES ('Obadiah');
INSERT INTO `Books` (`name`) VALUES ('Jonah');
INSERT INTO `Books` (`name`) VALUES ('Micah');
INSERT INTO `Books` (`name`) VALUES ('Nahum');
INSERT INTO `Books` (`name`) VALUES ('Habakkuk');
INSERT INTO `Books` (`name`) VALUES ('Zephaniah');
INSERT INTO `Books` (`name`) VALUES ('Haggai');
INSERT INTO `Books` (`name`) VALUES ('Zechariah');
INSERT INTO `Books` (`name`) VALUES ('Malachi');
INSERT INTO `Books` (`name`) VALUES ('Matthew');
INSERT INTO `Books` (`name`) VALUES ('Mark');
INSERT INTO `Books` (`name`) VALUES ('Luke');
INSERT INTO `Books` (`name`) VALUES ('John');
INSERT INTO `Books` (`name`) VALUES ('Acts');
INSERT INTO `Books` (`name`) VALUES ('Romans');
INSERT INTO `Books` (`name`) VALUES ('1 Corinthians');
INSERT INTO `Books` (`name`) VALUES ('2 Corinthians');
INSERT INTO `Books` (`name`) VALUES ('Galatians');
INSERT INTO `Books` (`name`) VALUES ('Ephesians');
INSERT INTO `Books` (`name`) VALUES ('Philippians');
INSERT INTO `Books` (`name`) VALUES ('Colossians');
INSERT INTO `Books` (`name`) VALUES ('1 Thessalonians');
INSERT INTO `Books` (`name`) VALUES ('2 Thessalonians');
INSERT INTO `Books` (`name`) VALUES ('1 Timothy');
INSERT INTO `Books` (`name`) VALUES ('2 Timothy');
INSERT INTO `Books` (`name`) VALUES ('Titus');
INSERT INTO `Books` (`name`) VALUES ('Philemon');
INSERT INTO `Books` (`name`) VALUES ('Hebrews');
INSERT INTO `Books` (`name`) VALUES ('James');
INSERT INTO `Books` (`name`) VALUES ('1 Peter');
INSERT INTO `Books` (`name`) VALUES ('2 Peter');
INSERT INTO `Books` (`name`) VALUES ('1 John');
INSERT INTO `Books` (`name`) VALUES ('2 John');
INSERT INTO `Books` (`name`) VALUES ('3 John');
INSERT INTO `Books` (`name`) VALUES ('Jude');
INSERT INTO `Books` (`name`) VALUES ('Revelation');
