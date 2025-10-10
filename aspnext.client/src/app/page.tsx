import Image from "next/image";
import styles from "./page.module.scss";

export default function Home() {
    return (
        <div className={styles.page}>
            <div className={styles.center}>
                <h1>This is a ASP.NET + Next.js template</h1>
            </div>
        </div>
    );
}
